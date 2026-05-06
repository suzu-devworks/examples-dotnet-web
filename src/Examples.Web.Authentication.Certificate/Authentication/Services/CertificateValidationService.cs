using System.Security.Cryptography.X509Certificates;

namespace Examples.Web.Authentication.Services;

public class CertificateValidationService : ICertificateValidationService
{
    public bool ValidateCertificate(X509Certificate2 clientCertificate)
    {
        return true;
    }
}
