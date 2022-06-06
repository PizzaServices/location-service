namespace Location_Service.Repositories.Locations;

public record Location
{
    public Guid Id;
    public string Hash = null!;
    public double Latitude;
    public double Longitude;
}