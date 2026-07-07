using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Kernel.Responses
{
    /// <summary>
    /// Interface of the response.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Response message.
        /// </summary>
        string? ResponseMessage { get; set; }
    }
}
