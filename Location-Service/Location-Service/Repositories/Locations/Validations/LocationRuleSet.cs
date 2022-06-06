using Location_Service.Repositories.Validations;

namespace Location_Service.Repositories.Locations.Validations;

public class LocationRuleSet : ILocationRuleSet
{
    public RuleSetValidationResponse ValidateLatitude(double latitude)
    {
        var response = new RuleSetValidationResponse();

        if (latitude < -90d || latitude > 90d)
        {
            response.ErrorMessage = "Latitude is not valid";
            return response;
        }

        response.Successful = true;
        return response;
    }

    public RuleSetValidationResponse ValidateLongitude(double longitude)
    {
        var response = new RuleSetValidationResponse();

        if (longitude < -180d || longitude > 180d)
        {
            response.ErrorMessage = "Longitude is not valid";
            return response;
        }

        response.Successful = true;
        return response;
    }
}