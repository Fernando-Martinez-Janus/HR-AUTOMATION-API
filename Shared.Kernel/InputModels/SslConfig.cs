namespace Shared.Kernel.InputModels
{
    /// <summary>
    /// Represents per-request SSL/TLS configuration for an HTTP call.
    /// </summary>
    /// <remarks>
    /// The caller is responsible for loading certificate data from disk or any other source
    /// before building this model. Options are not mutually exclusive —
    /// for example, you can send a client certificate AND trust a specific server cert.
    /// </remarks>
    public class SslConfig
    {
        /// <summary>
        /// When <c>true</c>, bypasses all server certificate validation.
        /// Use only for internal APIs with self-signed or untrusted certificates.
        /// </summary>
        public bool IgnoreAllErrors { get; set; }

        /// <summary>
        /// DER-encoded bytes of the trusted server certificate (public key only, e.g. from a .cer file).
        /// When set, the server certificate is accepted only if its thumbprint matches these bytes.
        /// Load the file with <c>File.ReadAllBytes</c> before assigning.
        /// </summary>
        public byte[]? TrustedCertificate { get; set; }

        /// <summary>
        /// PKCS#12 / PFX bytes of the client certificate used for mutual TLS (mTLS).
        /// The data must include the private key.
        /// Load the file with <c>File.ReadAllBytes</c> before assigning.
        /// </summary>
        public byte[]? ClientCertificate { get; set; }

        /// <summary>
        /// Password for the client certificate, if it is password-protected.
        /// </summary>
        public string? ClientCertificatePassword { get; set; }
    }
}
