using System.Security.Cryptography.X509Certificates;

namespace Examples.Web.Authentication;

public static class CertificateLoader
{
    private static readonly HashSet<string> CertificateExtensions = [".crt", ".cer", ".pem"];

    public static X509Certificate2Collection LoadCertificates(string customTrustStorePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customTrustStorePath, nameof(customTrustStorePath));

        var path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, customTrustStorePath));
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

        throw new DirectoryNotFoundException($"The custom trust store path '{path}' does not exist.");
    }
}
