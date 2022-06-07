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
        var response = new CreateLocationResponse();

        var validationResponse = _locationValidator.ValidateCreation(request);

        if (!validationResponse.Successful)
        {
            response.ErrorMessages = validationResponse.ErrorMessages;
            return response;
        }

        var record = _dataAccess.Create(request.Latitude, request.Longitude);

        // todo check record for null
        
        var entity = new Location
        {
            Id = record.Id,
            Latitude = record.Latitude,
            Longitude = record.Longitude,
        };

        response.Successful = true;
        response.Model = entity;
        return response;
    }
}