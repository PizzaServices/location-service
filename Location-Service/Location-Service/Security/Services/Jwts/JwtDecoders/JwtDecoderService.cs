using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Location_Service.Security.Services.Certificates;

namespace Location_Service.Security.Services.Jwts.JwtDecoders;

public class JwtDecoderService : IJwtDecoderService
{
    private readonly IJwtDecoder _decoder;

    public JwtDecoderService(ICertificateService certificateService)
    {
        var certificate = certificateService.GetX509Certificate();

        var serializer = new JsonNetSerializer();
        var provider = new UtcDateTimeProvider();
        var validationParameters = ValidationParameters.Default;
        var validator = new JwtValidator(serializer, provider, validationParameters);
        var urlEncoder = new JwtBase64UrlEncoder();
        var algorithm = new RS256Algorithm(certificate);

        _decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
    }

    public IJwtDecoder GetDecoder()
    {
        return _decoder; 
    }
}