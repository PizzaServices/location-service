namespace Location_Service.Security.Services.Jwts.JwtValidators;

public interface IJwtValidator
{
    JwtValidationResponse ValidateToken(string token);
}