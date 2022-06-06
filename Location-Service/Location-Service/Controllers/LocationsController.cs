using Geohash;
using Location_Service.Models.Locations;
using Location_Service.Models.Locations.Create;
using Location_Service.Repositories.Locations.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Location_Service.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationReadRepository _locationReadRepository;
    private readonly ILocationWriteRepository _locationWriteRepository;

    public LocationsController(ILocationReadRepository locationReadRepository,
                               ILocationWriteRepository locationWriteRepository)
    {
        _locationReadRepository = locationReadRepository;
        _locationWriteRepository = locationWriteRepository;
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
        var response = _locationWriteRepository.Create(new CreateLocationRequest
        {
            Latitude = requestModel.Latitude,
            Longitude = requestModel.Longitude,
        });

        LocationModel? model = null;

        if (response.Model != null)
        {
            model = ConvertEntityToModel(response.Model);
        }

        return new JsonResult(new CreateLocationResponseModel
        {
            Model = model,
            Successful = response.Successful,
            ErrorMessages = response.ErrorMessages,
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