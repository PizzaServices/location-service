using System.Collections.Concurrent;
using System.Reflection;
using Location_Service.Containers;
using Location_Service.DataAccesses.Locations.InMemory;
using Location_Service.Services.GeoHashers;

namespace Location_Service_Tests.DataAccesses.InMemory;

public class InMemoryLocationDataAccessTest
{
    private const double LATITUDE = 49.452184378285004d;
    private const double LONGITUDE = 11.083240923668871d;
    
    private Mock<IGeoHasher> _geoHasherMock;
    private string _hash;

    private ISearchTree<Guid, InMemoryLocationRecord> _locationRecords;
    private ISearchTree<string, ConcurrentBag<Guid>> _hashBucket;

    [SetUp]
    public void SetUp()
    {
        _locationRecords = null;
        _hashBucket = null;
    }

    [Test]
    public void ReturnLocationForId()
    {
        // Arrange
        var access = GetAccess();
        var guid = Guid.NewGuid();
        var locationRecord = new InMemoryLocationRecord();
        _locationRecords.Insert(guid, locationRecord);
        
        // Act
        var record = access.GetById(guid);
        
        // Assert
        Assert.That(record, Is.EqualTo(locationRecord));
    }

    [Test]
    public void ShouldReturnNullIfIdDoesNotExist()
    {
        // Arrange
        var access = GetAccess();
        
        // Act
        var record = access.GetById(Guid.NewGuid());
        
        // Assert
        Assert.That(record, Is.Null);
    }

    [Test]
    public void ShouldCreateLocationWithNewHashBucket()
    {
        // Arrange
        _hash = "Hash";
        var access = GetAccess();
        
        // Act
        var record = access.Create(50d, 60d);
        
        // Assert
        Assert.That(record.Id, Is.Not.Empty);
        Assert.That(record.Latitude, Is.EqualTo(50d));
        Assert.That(record.Longitude, Is.EqualTo(60d));
        
        _geoHasherMock.Verify(hasher => hasher.Encode(50d, 60d), Times.Once);

        var locationRecord = _locationRecords.Get(record.Id);
        Assert.That(record, Is.Not.Null);

        var hashBucket = _hashBucket.Get(_hash);
        Assert.That(hashBucket, Is.Not.Null);
        Assert.That(hashBucket, Has.Count.EqualTo(1));
    }

    [Test]
    public void ShouldCreateLocationInExistingHashBucket()
    {
        _hash = "Hash";
        var access = GetAccess();
        
        // Act
        access.Create(50d, 60d);
        access.Create(70d, 80d);
        
        // Assert
        _geoHasherMock.Verify(hasher => hasher.Encode(50d, 60d), Times.Once);
        _geoHasherMock.Verify(hasher => hasher.Encode(70d, 80d), Times.Once);

        Assert.That(_locationRecords.Size(), Is.EqualTo(2));

        var hashBucket = _hashBucket.Get(_hash);
        Assert.That(hashBucket, Is.Not.Null);
        Assert.That(hashBucket, Has.Count.EqualTo(2));
    }

    [Test]
    public void ShouldReturnEmptyListIfNoHashBucketIsAvailableForRadiusSearch()
    {
        // Arrange
        var access = GetAccess();

        // Act
        var result = access.GetByRadius(LATITUDE, LONGITUDE, 100);

        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    public void ShouldReturnEmptyListIfNoGuidInHashBucketIsAvailableForRadiusSearch()
    {
        // Arrange
        var access = GetAccess();
        _hashBucket.Insert("u0zck4w", new ConcurrentBag<Guid>());

        // Act
        var result = access.GetByRadius(LATITUDE, LONGITUDE, 100);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ShouldReturnEmptyListIfNoLocationForGuidExists()
    {
        // Arrange
        var access = GetAccess();
        _hashBucket.Insert("u0zck4w", new ConcurrentBag<Guid>
        {
            Guid.NewGuid(),
        });
        
        // Act
        var result = access.GetByRadius(LATITUDE, LONGITUDE, 100);
        
        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [Test]
    [TestCase("u0zck4w")]
    [TestCase("u0zck4q")]
    [TestCase("u0zck4x")]
    [TestCase("u0zck4r")]
    [TestCase("u0zck4t")]
    [TestCase("u0zck4m")]
    public void ShouldReturnLocationsForRadiusSearch(string hash)
    {
        // Arrange
        var access = GetAccess();
        var guid = Guid.NewGuid();
        _hashBucket.Insert(hash, new ConcurrentBag<Guid>
        {
            guid
        });
        var locationRecord = new InMemoryLocationRecord();
        _locationRecords.Insert(guid, locationRecord);
        
        // Act
        var result = access.GetByRadius(LATITUDE, LONGITUDE, 100);
        
        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(locationRecord));
    }
    
    private InMemoryLocationDataAccess GetAccess()
    {
        _geoHasherMock = new Mock<IGeoHasher>();
        _geoHasherMock
            .Setup(hasher => hasher.Encode(It.IsAny<double>(), It.IsAny<double>()))
            .Returns(() => _hash);
        
        var dataAccess = new InMemoryLocationDataAccess(_geoHasherMock.Object);
        
        _locationRecords = typeof(InMemoryLocationDataAccess)
                                .GetField("_locationRecords", BindingFlags.NonPublic | BindingFlags.Instance)
                                ?.GetValue(dataAccess) as ISearchTree<Guid, InMemoryLocationRecord>;
        
        _hashBucket = typeof(InMemoryLocationDataAccess)
                            .GetField("_hashBucket", BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.GetValue(dataAccess) as ISearchTree<string, ConcurrentBag<Guid>>;

        return dataAccess;
    }
}