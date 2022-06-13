using System.Security.Cryptography.X509Certificates;

namespace Location_Service.Security.Services.Certificates;

public class CertificateService : ICertificateService
{
    private readonly X509Certificate2 _certificate;

    private CertificateService(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

    public static CertificateService CreateNewCertificate(string path)
    {
        var certificate = new X509Certificate2(path);
        return new CertificateService(certificate);
    }
    
    public X509Certificate2 GetX509Certificate()
    {
        return _certificate;
    }
}