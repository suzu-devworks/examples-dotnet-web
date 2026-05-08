using System.Security.Cryptography.X509Certificates;

namespace Examples.Web.Authentication;

public interface ICertificateValidationService
{
    bool ValidateCertificate(X509Certificate2 clientCertificate);
}
