using Location_Service.DataAccesses.Locations;
using Location_Service.DataAccesses.Locations.InMemory;
using Location_Service.Repositories.Locations;
using Location_Service.Services.LocationCreationServices;

namespace Location_Service.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddServices(this IServiceCollection serviceCollection)
    {
        // Singleton
        serviceCollection.AddSingleton<ILocationDataAccess, LocationDataAccess>();
        
        // Scoped
        serviceCollection.AddScoped<ILocationCreationService, LocationCreationService>();
        serviceCollection.AddScoped<ILocationReadRepository, LocationReadRepository>();
        serviceCollection.AddScoped<ILocationWriteRepository, LocationWriteRepository>();

        // Transient 
    }
}