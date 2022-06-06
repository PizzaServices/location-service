using Geohash;
using Location_Service.Models.Locations;
using Location_Service.Models.Locations.Create;
using Location_Service.Repositories.Locations;
using Location_Service.Services.LocationCreationServices;
using Microsoft.AspNetCore.Mvc;

namespace Location_Service.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationReadRepository _locationReadRepository;
    private readonly ILocationCreationService _locationCreationService;

    public LocationsController(ILocationReadRepository locationReadRepository,
                               ILocationCreationService locationCreationService)
    {
        _locationReadRepository = locationReadRepository;
        _locationCreationService = locationCreationService;
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var location = _locationReadRepository.Get(id);

        if (location == null)
            return NotFound();

        return new JsonResult(new LocationModel
        {
            Id = location.Id,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
        });
    }

    [HttpGet]
    public IActionResult Get(double latitude, double longitude, int radius)
    {
        var geoHasher = new Geohasher();
        var locations = _locationReadRepository.GetByRadius(latitude, longitude, radius);

        return new JsonResult(new
        {
            Latitude = latitude,
            Longitude = longitude,
            Locations = locations.Select(ConvertEntityToModel),
        });
    }

    [HttpPut]
    public IActionResult Create([FromBody] CreateLocationRequestModel requestModel)
    {
        var result = _locationCreationService.Create(new CreateLocationRequest
        {
            Latitude = requestModel.Latitude,
            Longitude = requestModel.Longitude,
        });

        LocationModel? model = null;

        if (result.Entity != null)
        {
            model = ConvertEntityToModel(result.Entity);
        }

        return new JsonResult(new CreateLocationResponseModel
        {
            Model = model,
            Successful = result.Successful,
            ErrorMessages = result.ErrorMessages,
        });
    }

    private static LocationModel ConvertEntityToModel(Location location)
    {
        return new LocationModel
        {
            Id = location.Id,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
        };
    }
}