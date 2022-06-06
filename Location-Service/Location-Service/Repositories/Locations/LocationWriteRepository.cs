using Geohash;
using Location_Service.DataAccesses.Locations;
using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Locations.Validations;

namespace Location_Service.Repositories.Locations;

public class LocationWriteRepository : ILocationWriteRepository
{
    private readonly ILocationDataAccess _dataAccess;
    private readonly ILocationValidator _locationValidator;

    public LocationWriteRepository(ILocationDataAccess dataAccess,
                                   ILocationValidator locationValidator)
    {
        _dataAccess = dataAccess;
        _locationValidator = locationValidator;
    }
    
    public CreateLocationResponse Create(CreateLocationRequest request)
    {
        var record = _dataAccess.Create(request.Latitude, request.Longitude);

        // return new Location
        // {
        //     Id = record.Id,
        //     Latitude = request.Latitude,
        //     Longitude = request.Longitude,
        // };

        return null;
    }
}