using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Tokens;

namespace Examples.Web.Generator.JwtToken.Utilities;

/// <summary>
/// Utility class for loading certificates and extracting signing credentials or public keys for JWT token generation and verification.
/// </summary>
public static class CertificateLoader
{
    /// <summary>
    /// Loads signing credentials from a PFX/P12 certificate file. Supports both RSA and ECDSA keys.
    /// </summary>
    /// <param name="path">The path to the certificate file.</param>
    /// <param name="passwordProvider">A function that provides the password for the certificate file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the signing credentials.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<SigningCredentials> LoadSigningCredentialsAsync(string path, Func<string> passwordProvider,
        CancellationToken cancellationToken = default)
    {
        using var certificate = await LoadCertificateFromFileAsync(path, passwordProvider, cancellationToken);
        return GetSigningCredentials(certificate);
    }

    /// <summary>
    /// Loads a PKCS#12 (PFX/P12) certificate from a file asynchronously, using the provided password.
    /// </summary>
    /// <param name="path">The path to the certificate file.</param>
    /// <param name="passwordProvider">A function that provides the password for the certificate file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded certificate.</returns>
    public static Task<X509Certificate2> LoadPkcs12FromFileAsync(string path, Func<string> passwordProvider,
        CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var password = passwordProvider();
            return X509CertificateLoader.LoadPkcs12FromFile(path, password, X509KeyStorageFlags.EphemeralKeySet);
        }, cancellationToken);
    }

    private static SigningCredentials GetSigningCredentials(X509Certificate2 certificate)
    {
        // Identify the key type and create credentials.
        using var ecdsa = certificate.GetECDsaPrivateKey();
        if (ecdsa is not null)
        {
            var key = new ECDsaSecurityKey(ECDsa.Create(ecdsa.ExportParameters(true)))
            {
                KeyId = certificate.Thumbprint
            };

            return new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);
        }

        using var rsa = certificate.GetRSAPrivateKey();
        if (rsa is not null)
        {
            var key = new RsaSecurityKey(RSA.Create(rsa.ExportParameters(true)))
            {
                KeyId = certificate.Thumbprint
            };

            return new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
        }

        throw new InvalidOperationException("The private key could not be found.");
    }

    /// <summary>
    /// Loads signing credentials (public key only) from a certificate file (supports .cer, .crt, .pem, and .pfx formats) for JWT token verification. Supports both RSA and ECDSA keys.
    /// </summary>
    /// <param name="path">The path to the certificate file.</param>
    /// <param name="passwordProvider">A function that provides the password for the certificate file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the signing credentials with the public key and algorithm.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<SigningCredentials> LoadPublicSigningCredentialsAsync(string path, Func<string> passwordProvider,
        CancellationToken cancellationToken = default)
    {
        // Load certificate and extract public key as signing credentials
        using var certificate = await LoadCertificateFromFileAsync(path, passwordProvider, cancellationToken);
        return GetPublicSigningCredentials(certificate);
    }

    /// <summary>
    /// Loads a certificate from a file, supporting both PFX/P12 (with password) and CER/CRT/PEM formats.
    /// </summary>
    /// <param name="path">The path to the certificate file.</param>
    /// <param name="passwordProvider">A function that provides the password for the certificate file.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the loaded certificate.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<X509Certificate2> LoadCertificateFromFileAsync(string path, Func<string> passwordProvider,
        CancellationToken cancellationToken = default)
    {
        // Determine the certificate type based on the file content
        byte[] certData = await File.ReadAllBytesAsync(path, cancellationToken);
        var type = X509Certificate2.GetCertContentType(certData);

        if (type == X509ContentType.Pkcs12 || type == X509ContentType.Pfx)
        {
            var password = passwordProvider();
            return X509CertificateLoader.LoadPkcs12(certData, password, X509KeyStorageFlags.EphemeralKeySet);
        }
        else if (type == X509ContentType.Cert)
        {
            return X509CertificateLoader.LoadCertificate(certData);
        }
        else
        {
            throw new InvalidOperationException("Unsupported certificate format. Please provide a .cer, .crt, or .pem file.");
        }
    }

    private static SigningCredentials GetPublicSigningCredentials(X509Certificate2 certificate)
    {
        using var ecdsa = certificate.GetECDsaPublicKey();
        if (ecdsa is not null)
        {
            var key = new ECDsaSecurityKey(ECDsa.Create(ecdsa.ExportParameters(false)))
            {
                KeyId = certificate.Thumbprint
            };
            return new SigningCredentials(key, SecurityAlgorithms.EcdsaSha256);
        }

        using var rsa = certificate.GetRSAPublicKey();
        if (rsa is not null)
        {
            var key = new RsaSecurityKey(RSA.Create(rsa.ExportParameters(false)))
            {
                KeyId = certificate.Thumbprint
            };
            return new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
        }

        throw new InvalidOperationException("The public key could not be found.");
    }

}
