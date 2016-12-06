// ***********************************************************************
// Assembly         : WatsonSTT.Net
// Author           : jzielke
// Created          : 12-03-2016
//
// Last Modified By : jzielke
// Last Modified On : 12-05-2016
// ***********************************************************************
// <copyright file="WatsonSTTClient.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace WatsonSTT.Net
{
    /// <summary>
    /// Class WatsonSTTClient.
    /// </summary>
    public class WatsonSTTClient
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        private String URL { get; set; }

        /// <summary>
        /// Gets or sets the query string.
        /// </summary>
        /// <value>The query string.</value>
        public NameValueCollection QueryString { get; set; }

        /// <summary>
        /// Gets or sets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers { get; set; }

        /// <summary>
        /// Gets or sets the voice file to upload.
        /// </summary>
        /// <value>The voice file.</value>
        private String VoiceFile { get; set; }

        /// <summary>
        /// Gets or sets the type of the content type.
        /// </summary>
        /// <value>The type of the content.</value>
        public String ContentType { get; set; }

        /// <summary>
        /// The credentials
        /// </summary>
        private NetworkCredential Credentials = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="WatsonSTTClient" /> class.
        /// </summary>
        /// <param name="URL">The URL.</param>
        public WatsonSTTClient(string URL)
        {
            this.URL = URL;
            QueryString = new NameValueCollection();
            Headers = new NameValueCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatsonSTTClient" /> class.
        /// </summary>
        /// <param name="URL">The URL.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="Password">The password.</param>
        public WatsonSTTClient(string URL, string UserName, string Password)
            : this(URL)
        {
            this.Credentials = new NetworkCredential(UserName, Password);
        }


        /// <summary>
        /// Speeches to text.
        /// </summary>
        /// <param name="VoiceFile">The voice file.</param>
        /// <returns>WatsonSTTResponse.</returns>
        public WatsonSTTResponse SpeechToText(String VoiceFile)
        {
            this.VoiceFile = VoiceFile;

            var Response = Send("POST", URL);
            if (Response == null)
                return null;

            return JsonConvert.DeserializeObject<WatsonSTTResponse>(Response);
        }

        /// <summary>
        /// Sends the specified method.
        /// </summary>
        /// <param name="Method">The method.</param>
        /// <param name="URL">The URL.</param>
        /// <returns>String.</returns>
        private String Send(String Method, String URL)
        {
            try
            {
                FileStream VoiceFS = null;

                Uri lUri = new Uri(URL + QueryString.ToQueryString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(lUri) as HttpWebRequest;
                request.WithCredentials(Credentials).Method = Method;
                request.AllowAutoRedirect = false;

                request.ContentType = ContentType;

                using (VoiceFS = new FileStream(VoiceFile, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = null;
                    int bytesRead = 0;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        /*
                         * Read 1024 raw bytes from the input audio file.
                         */
                        buffer = new Byte[checked((uint)Math.Min(1024, (int)VoiceFS.Length))];
                        while ((bytesRead = VoiceFS.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                        }

                        // Flush
                        requestStream.Flush();
                    }
                }

                string responseString = "";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // (((HttpWebResponse)response).StatusCode);

                    using (Stream ResponseST = response.GetResponseStream())
                    {
                        byte[] data = new byte[4096];
                        int read;
                        while ((read = ResponseST.Read(data, 0, data.Length)) > 0)
                        {
                            responseString += System.Text.Encoding.Default.GetString(data);
                        }
                    }
                }

                return responseString;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
