using Location_Service.DataAccesses.Locations;
using Location_Service.Repositories.Locations;
using Location_Service.Repositories.Locations.Contracts;
using Location_Service.Repositories.Locations.Validations;
using Location_Service.Repositories.Validations;

namespace Location_Service_Tests.Repositories.Locations;

public class LocationWriteRepositoryTest
{
    private Mock<ILocationDataAccess> _dataAccessMock;
    private Mock<ILocationValidator> _locationValidator;

    private LocationRecord _locationRecord;
    private ValidationResponse _validationResponse;

    private CreateLocationRequest _createLocationRequest;
    
    [SetUp]
    public void SetUp()
    {
        _validationResponse = new ValidationResponse
        {
            Successful = true,
            ErrorMessages = new List<string>(),
        };

        _locationRecord = new LocationRecord
        {
            Id = Guid.NewGuid(),
            Latitude = 50d,
            Longitude = 60d,
        };

        _createLocationRequest = new CreateLocationRequest
        {
            Latitude = 70d,
            Longitude = 80d,
        };
    }

    [Test]
    public void ShouldCreateLocation()
    {
        // Arrange
        var repo = GetRepo();
        
        // Act
        var response = repo.Create(_createLocationRequest);
        
        // Assert
        Assert.That(response.Successful, Is.True);
        Assert.That(response.ErrorMessages.Any, Is.False);
        Assert.That(response.Model, Is.Not.Null);
        Assert.That(response.Model.Id, Is.EqualTo(_locationRecord.Id));
        Assert.That(response.Model.Latitude, Is.EqualTo(_locationRecord.Latitude));
        Assert.That(response.Model.Longitude, Is.EqualTo(_locationRecord.Longitude));
        
        _locationValidator.Verify(validator => validator.ValidateCreation(_createLocationRequest), Times.Once);
        _locationValidator.VerifyNoOtherCalls();
        
        _dataAccessMock.Verify(access => access.Create(70d, 80d), Times.Once);
        _dataAccessMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldReturnFailerOnCreationIfValidationFails()
    {
        // Arrange
        const string errorMessage = "Test Error Message";
        
        _validationResponse.Successful = false;
        _validationResponse.ErrorMessages.Add(errorMessage);
        var repo = GetRepo();
        
        // Act
        var response = repo.Create(_createLocationRequest);
        
        // Assert
        Assert.That(response.Successful, Is.False);
        Assert.That(response.ErrorMessages.Any, Is.True);
        Assert.That(response.ErrorMessages[0], Is.EqualTo(errorMessage));
        Assert.That(response.Model, Is.Null);

        _locationValidator.Verify(validator => validator.ValidateCreation(_createLocationRequest), Times.Once);
        _locationValidator.VerifyNoOtherCalls();
        
        _dataAccessMock.VerifyNoOtherCalls();
    }
    
    private LocationWriteRepository GetRepo()
    {
        _locationValidator = new Mock<ILocationValidator>();
        _locationValidator
            .Setup(validator => validator.ValidateCreation(It.IsAny<CreateLocationRequest>()))
            .Returns(_validationResponse);

        _dataAccessMock = new Mock<ILocationDataAccess>();
        _dataAccessMock
            .Setup(access => access.Create(It.IsAny<double>(), It.IsAny<double>()))
            .Returns(_locationRecord);

        return new LocationWriteRepository(_dataAccessMock.Object, _locationValidator.Object);
    }
}