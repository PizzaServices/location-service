namespace Location_Service.Repositories.Locations;

public interface ILocationReadRepository
{
    Location? Get(Guid id);

    IEnumerable<Location> GetByRadius(double lat, double lon, int meters);
}