using System.Collections.Concurrent;
using Location_Service.Containers;

namespace Location_Service.DataAccesses.Locations.InMemory;

public class LocationDataAccess : ILocationDataAccess
{
    private readonly ISearchTree<Guid, LocationRecord> _locationRecords;
    private readonly ISearchTree<string, ConcurrentBag<Guid>> _hashBucket;

    public LocationDataAccess()
    {
        _locationRecords = new BinarySearchTree<Guid, LocationRecord>();
        _hashBucket = new BinarySearchTree<string, ConcurrentBag<Guid>>();
    }

    // Read
    public LocationRecord? GetById(Guid id)
    {
        return _locationRecords.Get(id);
    }

    public List<LocationRecord> GetByRadius(double lat, double lon, int meters)
    {
        var hashes = ProximityHash.GetGeoHashRadiusApproximation(lat,
            lon,
            meters,
            7);

        var result = new List<LocationRecord>();

        foreach (string hash in hashes)
        {
            var guids = _hashBucket.Get(hash);

            if (guids == null || guids.Count == 0)
                continue;

            foreach (var guid in guids)
            {
                var location = _locationRecords.Get(guid);
                if (location != null)
                    result.Add(location);
            }
        }

        return result;
    }

    // Write
    public LocationRecord Create(string hash, double latitude, double longitude)
    {
        var record = new LocationRecord
        {
            Hash = hash,
            Id = Guid.NewGuid(),
            Latitude = latitude,
            Longitude = longitude,
        };

        _locationRecords.Insert(record.Id, record);

        if (_hashBucket.Contains(record.Hash))
            _hashBucket.Get(record.Hash)!.Add(record.Id);
        else
            _hashBucket.Insert(record.Hash, new ConcurrentBag<Guid>{ record.Id });

        return record;
    }
}