using Location_Service.Extensions;
using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Validations;

namespace Location_Service.Repositories.Locations.Validations;

public class LocationValidator : ILocationValidator
{
    private readonly ILocationRuleSet _locationRuleSet;

    public LocationValidator(ILocationRuleSet locationRuleSet)
    {
        _locationRuleSet = locationRuleSet;
    }
    
    public ValidationResponse ValidateCreation(CreateLocationRequest request)
    {
        var response = new ValidationResponse();

        response.ErrorMessages.AddRuleSetValidationResponseIfNotSuccessful(
            _locationRuleSet.ValidateLatitude(request.Latitude));
        
        response.ErrorMessages.AddRuleSetValidationResponseIfNotSuccessful(
            _locationRuleSet.ValidateLongitude(request.Longitude));

        response.Successful = !response.ErrorMessages.Any();
        return response;
    }
}