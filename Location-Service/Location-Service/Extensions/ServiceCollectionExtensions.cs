using Location_Service.DataAccesses.Locations;
using Location_Service.DataAccesses.Locations.InMemory;
using Location_Service.Middleware;
using Location_Service.Repositories.Locations;
using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Locations.Validations;
using Location_Service.Security.Services.Certificates;
using Location_Service.Security.Services.Jwts.JwtDecoders;
using Location_Service.Security.Services.Jwts.JwtValidators;
using Location_Service.Services.GeoHashers;

namespace Location_Service.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        // Singleton
        serviceCollection.AddSingleton<ILocationDataAccess, InMemoryLocationDataAccess>();
        serviceCollection.AddSingleton<IGeoHasher, GeoHasher>();
        serviceCollection.AddSingleton<ICertificateService>(_ => CertificateService.CreateNewCertificate("./Resources/jwt_public_key.cert"));
        serviceCollection.AddSingleton<IJwtDecoderService, JwtDecoderService>();

        // Scoped
        serviceCollection.AddScoped<ILocationReadRepository, LocationReadRepository>();
        serviceCollection.AddScoped<ILocationWriteRepository, LocationWriteRepository>();

        serviceCollection.AddScoped<ILocationRuleSet, LocationRuleSet>();
        serviceCollection.AddScoped<ILocationValidator, LocationValidator>();

        serviceCollection.AddScoped<IJwtValidator, JwtValidator>();
        
        serviceCollection.AddScoped<RequestTimeLoggingMiddleware>();

        // Transient 
    }

    public static void AddJwtServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<JwtMiddleware>();
    }
}