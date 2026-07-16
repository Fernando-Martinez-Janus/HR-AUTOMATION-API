namespace Shared.Kernel.Utils.Constants
{
    /// <summary>
    /// Provides constants for commonly used HTTP MIME (media) types.
    /// </summary>
    public static class MediaTypes
    {
        /// <summary>
        /// Represents the MIME type for JSON content.
        /// </summary>
        public const string Json = "application/json";

        /// <summary>
        /// Represents the MIME type for XML content.
        /// </summary>
        public const string Xml = "application/xml";

        /// <summary>
        /// Represents the MIME type for plain text content.
        /// </summary>
        public const string Text = "text/plain";

        /// <summary>
        /// Represents the MIME type for multipart form data.
        /// </summary>
        public const string FormData = "multipart/form-data";

        /// <summary>
        /// Represents the MIME type for generic binary data.
        /// </summary>
        public const string OctetStream = "application/octet-stream";
    }
}