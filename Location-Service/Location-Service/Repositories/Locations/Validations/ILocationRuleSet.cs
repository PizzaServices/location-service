using Location_Service.Repositories.Validations;

namespace Location_Service.Repositories.Locations.Validations;

public interface ILocationRuleSet
{
   RuleSetValidationResponse ValidateLatitude(double latitude);
   RuleSetValidationResponse ValidateLongitude(double longitude);
}