using Shared.Kernel.Utils.Enums;

namespace Shared.Kernel.Responses
{
    /// <summary>
    /// Response of an exception.
    /// </summary>
    /// <remarks>
    /// Constructor using an HTTP code and a thrown exception.
    /// </remarks>
    /// <param name="code">HTTP status code.</param>
    /// <param name="responseMessage">Response message.</param>
    public class ResponseExceptionFactory : Exception, IResponse
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Response message.
        /// </summary>
        public string? ResponseMessage { get; set; }

        /// <summary>
        /// Constructor using a code and a message.
        /// </summary>
        /// <param name="code">Status code.</param>
        /// <param name="responseMessage">Response message.</param>
        public ResponseExceptionFactory(int code, string responseMessage)
        {
            Code = code;
            ResponseMessage = responseMessage;
        }

        /// <summary>
        /// Constructor using an HTTP code and a thrown exception.
        /// </summary>
        /// <param name="code">HTTP status code.</param>
        /// <param name="exception">Thrown exception.</param>
        public ResponseExceptionFactory(int code, Exception exception)
        {
            Code = code;
            ResponseMessage = exception.Message;
        }

        /// <summary>
        /// Constructor using an Exception enum.
        /// </summary>
        /// <param name="exception">Exception enum,</param>
        public ResponseExceptionFactory(Exceptions exception)
        {
            Code = exception.GetValue();
            ResponseMessage = exception.GetDescription();
        }

        /// <summary>
        /// Constructor using an instance of IResponse
        /// </summary>
        /// <param name="response">IResponse instance.</param>
        public ResponseExceptionFactory(IResponse response)
        {
            Code = response.Code;
            ResponseMessage = response.ResponseMessage;
        }
    }
}
