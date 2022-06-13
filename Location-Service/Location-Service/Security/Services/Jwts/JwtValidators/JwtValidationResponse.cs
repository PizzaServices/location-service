namespace Location_Service.Security.Services.Jwts.JwtValidators;

public enum JwtValidationResponse
{
    Valid = 0,
    TokenNotJetValid = 1,
    TokenExpired = 2,
    SignatureInvalid = 3,
    InvalidToken = 4,
}