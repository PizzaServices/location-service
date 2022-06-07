namespace Location_Service.Models.Locations;

public class RadiusSearchModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public IEnumerable<LocationModel> Locations { get; set; } = new List<LocationModel>();
}