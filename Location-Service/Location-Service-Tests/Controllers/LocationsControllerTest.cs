using Location_Service.Controllers;
using Location_Service.Models.Locations;
using Location_Service.Models.Locations.Create;
using Location_Service.Repositories.Locations.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Location_Service_Tests.Controllers;

public class LocationsControllerTest
{
    private Mock<ILocationReadRepository> _locationReadRepositoryMock;
    private Mock<ILocationWriteRepository> _locationWriteRepositoryMock;

    private Location _location;
    private List<Location> _locations;

    private CreateLocationResponse _createLocationResponse;
    private CreateLocationRequest _createLocationRequestCallback;

    [SetUp]
    public void SetUp()
    {
        _location = new Location
        {
            Id = Guid.NewGuid(),
            Latitude = 50d,
            Longitude = 60d,
        };

        _locations = new List<Location>
        {
            _location,
            new()
            {
                Id = Guid.NewGuid(),
                Latitude = 70d,
                Longitude = 80d,
            }
        };

        _createLocationResponse = new CreateLocationResponse
        {
            Successful = true,
            ErrorMessages = new List<string>(),
            Model = new Location
            {
                Id = new Guid(),
                Latitude = 90d,
                Longitude = 100d,
            },
        };

        _createLocationRequestCallback = null;
    }

    [Test]
    public void ShouldReturnLocationForGet()
    {
        // Arrange
        var controller = GetController();
        var guid = Guid.NewGuid();
        _location.Id = guid;
        
        // Act
        var result = controller.Get(guid);
        
        // Assert
        Assert.That(result, Is.TypeOf<JsonResult>());

        var model = typeof(JsonResult)
                        !.GetProperty("Value")
                        !.GetValue(result) as LocationModel;
        Assert.That(model.Id, Is.EqualTo(guid));
        Assert.That(model.Latitude, Is.EqualTo(50d));
        Assert.That(model.Longitude, Is.EqualTo(60d));
        
        _locationReadRepositoryMock.Verify(repo => repo.Get(guid), Times.Once);
        _locationReadRepositoryMock.VerifyNoOtherCalls();
        
        _locationWriteRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldReturnNotFoundForGetByIdIfLocationIsNotAvailable()
    {
        // Arrange
        _location = null;
        var controller = GetController();
        
        // Act
        var result = controller.Get(Guid.NewGuid());
        
        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
        
        _locationReadRepositoryMock.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
        _locationReadRepositoryMock.VerifyNoOtherCalls();
        
        _locationWriteRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldReturnLocationsWithinRadius()
    {
        // Arrange
        var guidOne = Guid.NewGuid();
        var guidTwo = Guid.NewGuid();
        _location.Id = guidOne;
        _locations[1].Id = guidTwo;
        var controller = GetController();
        
        // Act
        var result = controller.Get(10d, 20d, 42);
        
        // Assert
        Assert.That(result, Is.TypeOf<JsonResult>());

        var model = typeof(JsonResult)
            !.GetProperty("Value")
            !.GetValue(result) as RadiusSearchModel;
        
        Assert.That(model.Latitude, Is.EqualTo(10d));
        Assert.That(model.Longitude, Is.EqualTo(20d));
        
        Assert.That(model.Locations.Count(), Is.EqualTo(2));
        
        Assert.That(model.Locations.First().Id, Is.EqualTo(guidOne));
        Assert.That(model.Locations.First().Latitude, Is.EqualTo(50d));
        Assert.That(model.Locations.First().Longitude, Is.EqualTo(60d));
        
        Assert.That(model.Locations.Last().Id, Is.EqualTo(guidTwo));
        Assert.That(model.Locations.Last().Latitude, Is.EqualTo(70d));
        Assert.That(model.Locations.Last().Longitude, Is.EqualTo(80d));
        
        
        _locationReadRepositoryMock.Verify(repo => repo.GetByRadius(10d, 20d, 42), Times.Once);
        _locationReadRepositoryMock.VerifyNoOtherCalls();
        
        _locationWriteRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldReturnModelIfCreationIsSuccessful()
    {
        // Arrange
        var guid = Guid.NewGuid();
        _createLocationResponse.Model.Id = guid;
        var controller = GetController();
        
        // Act
        var result = controller.Create(new CreateLocationRequestModel
        {
            Latitude = 10d,
            Longitude = 20d,
        });
        
        // Assert
        Assert.That(result, Is.TypeOf<JsonResult>());

        var model = typeof(JsonResult)
                        !.GetProperty("Value")
                        !.GetValue(result) as CreateLocationResponseModel;
        
        Assert.That(model.Successful, Is.True);
        Assert.That(model.ErrorMessages, Has.Count.EqualTo(0));
        
        Assert.That(model.Model.Id, Is.EqualTo(guid));
        Assert.That(model.Model.Latitude, Is.EqualTo(90d));
        Assert.That(model.Model.Longitude, Is.EqualTo(100d));
        
        Assert.That(_createLocationRequestCallback.Latitude, Is.EqualTo(10d));
        Assert.That(_createLocationRequestCallback.Longitude, Is.EqualTo(20d));
        
        _locationWriteRepositoryMock.Verify(repo => repo.Create(It.IsAny<CreateLocationRequest>()), Times.Once);
        _locationWriteRepositoryMock.VerifyNoOtherCalls();
        
        _locationReadRepositoryMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldSetModelToNullIfCreationFails()
    {
        // Arrange
        _createLocationResponse.Successful = false;
        _createLocationResponse.ErrorMessages.Add("ErrorMessage");
        _createLocationResponse.Model = null;
        var controller = GetController();
        
        // Act
        var result = controller.Create(new CreateLocationRequestModel
        {
            Latitude = 10d,
            Longitude = 20d,
        });
        
        // Assert
        Assert.That(result, Is.TypeOf<JsonResult>());

        var model = typeof(JsonResult)
            !.GetProperty("Value")
            !.GetValue(result) as CreateLocationResponseModel;
        
        Assert.That(model.Successful, Is.False);
        Assert.That(model.ErrorMessages, Has.Count.EqualTo(1));
        Assert.That(model.ErrorMessages.First(), Is.EqualTo("ErrorMessage"));
        Assert.That(model.Model, Is.Null);

        Assert.That(_createLocationRequestCallback.Latitude, Is.EqualTo(10d));
        Assert.That(_createLocationRequestCallback.Longitude, Is.EqualTo(20d));
        
        _locationWriteRepositoryMock.Verify(repo => repo.Create(It.IsAny<CreateLocationRequest>()), Times.Once);
        _locationWriteRepositoryMock.VerifyNoOtherCalls();
        
        _locationReadRepositoryMock.VerifyNoOtherCalls();
    }
    
    private LocationsController GetController()
    {
        _locationReadRepositoryMock = new Mock<ILocationReadRepository>();
        _locationReadRepositoryMock
            .Setup(repo => repo.Get(It.IsAny<Guid>()))
            .Returns(_location);

        _locationReadRepositoryMock
            .Setup(repo => repo.GetByRadius(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(_locations);

        _locationWriteRepositoryMock = new Mock<ILocationWriteRepository>();
        _locationWriteRepositoryMock
            .Setup(repo => repo.Create(It.IsAny<CreateLocationRequest>()))
            .Callback<CreateLocationRequest>(request => _createLocationRequestCallback = request)
            .Returns(_createLocationResponse);

        return new LocationsController(_locationReadRepositoryMock.Object, _locationWriteRepositoryMock.Object);
    }
}