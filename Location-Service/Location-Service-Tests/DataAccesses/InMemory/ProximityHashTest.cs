using Location_Service.DataAccesses.Locations.InMemory;

namespace Location_Service_Tests.DataAccesses.InMemory;

public class ProximityHashTest
{
    [Test]
    [TestCase(10000, 1, "u", "s", "g", "e")]
    [TestCase(10000, 2, "u3", "u2", "u1", "u0")]
    [TestCase(10000, 3, "u2b", "u28", "u0z", "u0x")]
    [TestCase(10000, 4, "u0zc", "u0zf", "u0zb")]
    [TestCase(2000, 5, "u0zck", "u0zc7")]
    [TestCase(500, 6, "u0zck6", "u0zck4", "u0zck7", "u0zck3", "u0zck5", "u0zck1")]
    [TestCase(100, 7, "u0zck4w", "u0zck4q", "u0zck4x", "u0zck4r", "u0zck4t", "u0zck4m")]
    [TestCase(10, 8, "u0zck4w8", "u0zck4qx", "u0zck4w2", "u0zck4qr")]
    public void ReturnGeoHashesForLocation(int radius, int precision, params string[] expectedHashes)
    {
        // Arrange + Act
        var actual = ProximityHash.GetGeoHashRadiusApproximation(49.452184378285004d, 
                                                                         11.083240923668871d,
                                                                         radius,
                                                                         precision);
        
        // Assert
        actual.Sort();
        var expected = expectedHashes.ToList();
        expected.Sort();

        Assert.That(actual, Is.EquivalentTo(expected));
    }
}