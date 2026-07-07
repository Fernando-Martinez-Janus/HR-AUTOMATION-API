namespace Shared.Kernel.Responses
{
    /// <summary>
    /// Generic response with Data Response.
    /// </summary>
    /// <typeparam name="TResult">The type of the response object that will be returned.</typeparam>
    public class Response<TResult> : Response
    {
        /// <summary>
        /// Data response.
        /// </summary>
        public TResult? DataResponse { get; set; }
    }
}
