using System.Security.Cryptography.X509Certificates;

namespace Examples.Web.Authentication;

public static class CertificateLoader
{
    private static readonly HashSet<string> CertificateExtensions = [".crt", ".cer", ".pem"];

    public static X509Certificate2Collection LoadCertificates(string? customTrustStorePath)
    {
        var path = Path.Combine(Environment.CurrentDirectory, customTrustStorePath ?? string.Empty);
        if (string.IsNullOrEmpty(customTrustStorePath))
        {
            throw new DirectoryNotFoundException($"The specified path '{customTrustStorePath}' does not exist.");
        }

        var store = new X509Certificate2Collection();

        if (File.Exists(path))
        {
            store.Add(X509CertificateLoader.LoadCertificateFromFile(path));
            return store;
        }

        if (Directory.Exists(path))
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => CertificateExtensions.Contains(Path.GetExtension(s).ToLower()));

            foreach (var file in files)
            {
                var cert = X509CertificateLoader.LoadCertificateFromFile(file);
                if (cert is not null)
                {
                    store.Add(cert);
                }
            }
            return store;
        }

        throw new DirectoryNotFoundException($"The specified path '{customTrustStorePath}' does not exist.");

    }
}
