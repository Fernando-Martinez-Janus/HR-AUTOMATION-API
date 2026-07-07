using Shared.Kernel.Utils.Enums;

namespace Shared.Kernel.Responses
{
    /// <summary>
    /// Response of an exception.
    /// </summary>
    public class ResponseFactory : IResponse
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
        /// Constructor using an HTTP code and a thrown exception.
        /// </summary>
        /// <param name="code">HTTP status code.</param>
        /// <param name="exception">Thrown exception.</param>
        public ResponseFactory(int code, Exception exception)
        {
            Code = code;
            ResponseMessage = exception.Message;
        }

        /// <summary>
        /// Constructor using an instance of IResponse
        /// </summary>
        /// <param name="response">IResponse instance.</param>
        public ResponseFactory(IResponse response)
        {
            Code = response.Code;
            ResponseMessage = response.ResponseMessage;
        }

        /// <summary>
        /// Constructor using an Exception enum.
        /// </summary>
        /// <param name="exception">Exception enum,</param>
        public ResponseFactory(Exceptions exception)
        {
            Code = exception.GetValue();
            ResponseMessage = exception.GetDescription();
        }
    }
}
