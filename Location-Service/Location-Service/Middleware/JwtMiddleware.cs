using Location_Service.Security.Services.Jwts.JwtValidators;

namespace Location_Service.Middleware;

public class JwtMiddleware : IMiddleware
{
    private readonly IJwtValidator _jwtValidator;

    public JwtMiddleware(IJwtValidator jwtValidator)
    {
        _jwtValidator = jwtValidator;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            var response = _jwtValidator.ValidateToken(token);

            if (response == JwtValidationResponse.Valid)
                context.Items["IsAuthorized"] = true;
            else
                context.Items["IsAuthorized"] = false;
        }

        await next(context);
    }
}