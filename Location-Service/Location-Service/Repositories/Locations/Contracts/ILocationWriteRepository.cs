namespace Location_Service.Repositories.Locations.Contracts;

public interface ILocationWriteRepository
{
    CreateLocationResponse Create(CreateLocationRequest request);
}