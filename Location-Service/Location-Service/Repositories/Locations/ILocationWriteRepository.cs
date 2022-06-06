namespace Location_Service.Repositories.Locations;

public interface ILocationWriteRepository
{
    Location? Create(string hash, double latitude, double longitude);
}