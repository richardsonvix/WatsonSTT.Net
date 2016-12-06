// ***********************************************************************
// Assembly         : WatsonSTT.Net
// Author           : jzielke
// Created          : 12-03-2016
//
// Last Modified By : jzielke
// Last Modified On : 12-05-2016
// ***********************************************************************
// <copyright file="WatsonSTTResponse.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WatsonSTT.Net
{

    /// <summary>
    /// Class Result.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets or sets the alternatives.
        /// </summary>
        /// <value>The alternatives.</value>
        public Alternative[] alternatives { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Result"/> is final.
        /// </summary>
        /// <value><c>true</c> if final; otherwise, <c>false</c>.</value>
        public bool final { get; set; }
    }

    /// <summary>
    /// Class Alternative.
    /// </summary>
    public class Alternative
    {
        /// <summary>
        /// Gets or sets the confidence.
        /// </summary>
        /// <value>The confidence.</value>
        public float confidence { get; set; }
        /// <summary>
        /// Gets or sets the transcript.
        /// </summary>
        /// <value>The transcript.</value>
        public string transcript { get; set; }
    }

    /// <summary>
    /// Class WatsonSTTResponse.
    /// </summary>
    public class WatsonSTTResponse
    {
        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public Result[] results { get; set; }
        /// <summary>
        /// Gets or sets the result_index.
        /// </summary>
        /// <value>The result_index.</value>
        public int result_index { get; set; }
    }


    /// <summary>
    /// Class WatsonSTTErrorResponse.
    /// </summary>
    public class WatsonSTTErrorResponse
    {
        /// <summary>
        /// Gets or sets the code_description.
        /// </summary>
        /// <value>The code_description.</value>
        public string code_description { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int code { get; set; }
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        public string error { get; set; }
    }
}
