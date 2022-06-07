using Location_Service.DataAccesses.Locations;
using Location_Service.DataAccesses.Locations.InMemory;
using Location_Service.Middleware;
using Location_Service.Repositories.Locations;
using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Locations.Validations;
using Location_Service.Services.GeoHashers;

namespace Location_Service.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        // Singleton
        serviceCollection.AddSingleton<ILocationDataAccess, InMemoryLocationDataAccess>();
        
        // Scoped
        serviceCollection.AddScoped<ILocationReadRepository, LocationReadRepository>();
        serviceCollection.AddScoped<ILocationWriteRepository, LocationWriteRepository>();

        serviceCollection.AddScoped<ILocationRuleSet, LocationRuleSet>();
        serviceCollection.AddScoped<ILocationValidator, LocationValidator>();
        
        serviceCollection.AddScoped<RequestTimeLoggingMiddleware>();

        serviceCollection.AddScoped<IGeoHasher, GeoHasher>();

        // Transient 
    }
}