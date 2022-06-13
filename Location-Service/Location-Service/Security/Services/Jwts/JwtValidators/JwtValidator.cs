using JWT;
using JWT.Exceptions;
using Location_Service.Security.Services.Jwts.JwtDecoders;

namespace Location_Service.Security.Services.Jwts.JwtValidators;

public class JwtValidator : IJwtValidator
{
    private readonly IJwtDecoderService _decoderService;

    public JwtValidator(IJwtDecoderService decoderService)
    {
        _decoderService = decoderService;
    }
    
    public JwtValidationResponse ValidateToken(string token)
    {
        try
        {
            var decoder = _decoderService.GetDecoder();
            // todo this is a little bit ugly but otherwise the token validation is skipped
            string? json = decoder.Decode(new JwtParts(token), (byte[])null, true);
            return JwtValidationResponse.Valid;
        }
        catch (InvalidTokenPartsException)
        {
            return JwtValidationResponse.InvalidToken;
        }
        catch (TokenExpiredException)
        {
            return JwtValidationResponse.TokenExpired;
        }
        catch (TokenNotYetValidException)
        {
            return JwtValidationResponse.TokenNotJetValid;
        }
        catch (SignatureVerificationException)
        {
            return JwtValidationResponse.SignatureInvalid;
        }
    }
}