using Location_Service.DataAccesses.Locations;
using Location_Service.Repositories.Locations;
using Location_Service.Repositories.Locations.Contracts;

namespace Location_Service_Tests.Repositories.Locations;

public class LocationReadRepositoryTest
{
    private Mock<ILocationDataAccess> _dataAccessMock;
    private LocationRecord? _locationRecord;
    private List<LocationRecord> _locationRecords;

    [SetUp]
    public void SetUp()
    {
        _locationRecord = new LocationRecord
        {
            Id = Guid.NewGuid(),
            Latitude = 50d,
            Longitude = 60d,
        };

        _locationRecords = new List<LocationRecord>
        {
            _locationRecord,
            new()
            {
                Id = Guid.NewGuid(),
                Latitude = 50d,
                Longitude = 60d,
            },
        };
    }

    [Test]
    public void ShouldReturnNullIfGetByIdReturnsNull()
    {
        // Arrange
        _locationRecord = null;
        var repo = GetRepo();
        
        // Act
        var location = repo.Get(Guid.NewGuid());
        
        // Assert
        Assert.That(location, Is.Null);
        
        _dataAccessMock.Verify(access => access.GetById(It.IsAny<Guid>()), Times.Once);
    }

    [Test]
    public void ShouldReturnLocationForGetById()
    {
        // Arrange
        var repo = GetRepo();
        
        // Act
        var location = repo.Get(_locationRecord!.Id);
        
        // Assert
        Assert.That(location, Is.Not.Null);
        AssertFields(location!, _locationRecord);
        
        _dataAccessMock.Verify(access => access.GetById(_locationRecord.Id), Times.Once);
        _dataAccessMock.VerifyNoOtherCalls();
    }

    [Test]
    public void ShouldReturnLocationsForGetByRadius()
    {
        // Arrange
        var repo = GetRepo();
        
        // Act
        var locations = repo.GetByRadius(0d, 10d, 500).ToList();
        
        // Assert
        Assert.That(locations.Count, Is.EqualTo(2));
        AssertFields(locations.First(), _locationRecords[0]);
        AssertFields(locations.Last(), _locationRecords[1]);
        
        _dataAccessMock.Verify(access => access.GetByRadius(0d, 10d, 500), Times.Once);
        _dataAccessMock.VerifyNoOtherCalls();
    }

    private static void AssertFields(Location actual, LocationRecord expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));
            Assert.That(actual.Latitude, Is.EqualTo(expected.Latitude));
            Assert.That(actual.Longitude, Is.EqualTo(expected.Longitude));
        });
    }

    private LocationReadRepository GetRepo()
    {
        _dataAccessMock = new Mock<ILocationDataAccess>();
        _dataAccessMock
            .Setup(access => access.GetById(It.IsAny<Guid>()))
            .Returns(_locationRecord);

        _dataAccessMock
            .Setup(access => access.GetByRadius(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(_locationRecords);

        return new LocationReadRepository(_dataAccessMock.Object);
    }
}