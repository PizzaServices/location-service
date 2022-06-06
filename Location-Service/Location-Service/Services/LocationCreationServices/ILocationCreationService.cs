namespace Location_Service.Services.LocationCreationServices;

public interface ILocationCreationService
{
    CreateLocationResponse Create(CreateLocationRequest request);
}