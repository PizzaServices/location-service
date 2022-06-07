namespace Location_Service.Repositories.Locations.Contracts;

public record Location
{
    public Guid Id;
    public double Latitude;
    public double Longitude;
}