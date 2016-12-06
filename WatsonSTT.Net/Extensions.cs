// ***********************************************************************
// Assembly         : WatsonSTT.Net
// Author           : jzielke
// Created          : 12-05-2016
//
// Last Modified By : jzielke
// Last Modified On : 12-05-2016
// ***********************************************************************
// <copyright file="Extensions.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WatsonSTT.Net
{
    /// <summary>
    /// Class Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// With the credentials.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="Credentials">The credentials.</param>
        /// <returns>HttpWebRequest.</returns>
        public static HttpWebRequest WithCredentials(this HttpWebRequest request, NetworkCredential Credentials)
        {
            if (Credentials != null)
            {
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(Credentials.UserName + ":" + Credentials.Password));
            }
            return request;
        }

        /// <summary>
        /// With the headers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="Headers">The headers.</param>
        /// <returns>HttpWebRequest.</returns>
        public static HttpWebRequest WithHeaders(this HttpWebRequest request, object Headers)
        {
            if (Headers != null)
            {
                foreach (var pair in Headers.ToDictionary())
                {
                    request.Headers.Add(pair.Key, pair.Value.ToString());
                }
            }
            return request;
        }

        /// <summary>
        /// To the dictionary.
        /// http://goo.gl/mDHNEa
        /// </summary>
        /// <param name="myObj">My object.</param>
        /// <returns>Dictionary&lt;System.String, System.Object&gt;.</returns>
        public static Dictionary<string, object> ToDictionary(this object myObj)
        {
            return myObj.GetType()
                .GetProperties()
                .Select(pi => new { Name = pi.Name, Value = pi.GetValue(myObj, null) })
                .Union(
                    myObj.GetType()
                    .GetFields()
                    .Select(fi => new { Name = fi.Name, Value = fi.GetValue(myObj) })
                 )
                .ToDictionary(ks => ks.Name, vs => vs.Value);
        }

        /// <summary>
        /// Converts a System.DateTime object to Unix timestamp
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>The Unix timestamp</returns>
        /// <example>
        ///   <code>
        /// var currentUnixTimestamp = DateTime.Now.ToUnixTimestamp();
        /// </code>
        /// </example>
        public static long ToUnixTimestamp(this DateTime date)
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0);
            TimeSpan unixTimeSpan = date - unixEpoch;

            return (long)unixTimeSpan.TotalSeconds;
        }

        /// <summary>
        /// To the query string.
        /// </summary>
        /// <param name="nvc">The NVC.</param>
        /// <returns>System.String.</returns>
        public static string ToQueryString(this NameValueCollection QueryCollection)
        {
            var array = (from QueryKey in QueryCollection.AllKeys
                         from QueryValue in QueryCollection.GetValues(QueryKey)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(QueryKey), HttpUtility.UrlEncode(QueryValue))).ToArray();
            return "?" + string.Join("&", array);
        }
    }
}
