using Geohash;
using Location_Service.Repositories.Locations;

namespace Location_Service.Services.LocationCreationServices;

public class LocationCreationService : ILocationCreationService
{
    private readonly Geohasher _geoHasher;
    private readonly ILocationWriteRepository _locationWriteRepository;

    public LocationCreationService(ILocationWriteRepository locationWriteRepository)
    {
        _geoHasher = new Geohasher();
        
        _locationWriteRepository = locationWriteRepository;
    }
    
    public CreateLocationResponse Create(CreateLocationRequest request)
    {
        var hash = _geoHasher.Encode(request.Latitude, request.Longitude, 9);

        if (hash == null)
        {
            return new CreateLocationResponse
            {
                Successful = false,
                ErrorMessages = new List<string>
                {
                    "Invalid latitude and longitude",
                }
            };
        }

        var location = _locationWriteRepository.Create(hash, request.Latitude, request.Longitude);

        if (location == null)
        {
            return new CreateLocationResponse
            {
                Successful = false,
                ErrorMessages = new List<string>
                {
                    "Could not create record.",
                }
            };
        }
        
        return new CreateLocationResponse
        {
            Successful = true,
            Entity = location,
        };
    }
}