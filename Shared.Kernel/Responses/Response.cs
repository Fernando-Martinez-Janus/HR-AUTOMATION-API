using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Kernel.Responses
{
    /// <summary>
    /// Generic response.
    /// </summary>
    public class Response : IResponse
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Response message.
        /// </summary>
        public string? ResponseMessage { get; set; }
    }
}
