using JWT;

namespace Location_Service.Security.Services.Jwts.JwtDecoders;

public interface IJwtDecoderService
{
    IJwtDecoder GetDecoder();
}