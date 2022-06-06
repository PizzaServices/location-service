using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Validations;

namespace Location_Service.Repositories.Locations.Validations;

public interface ILocationValidator
{
   ValidationResponse ValidateCreation(CreateLocationRequest request);
}