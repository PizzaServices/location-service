using System.Security.Cryptography.X509Certificates;

namespace Location_Service.Security.Services.Certificates;

public interface ICertificateService
{
    X509Certificate2 GetX509Certificate();
}