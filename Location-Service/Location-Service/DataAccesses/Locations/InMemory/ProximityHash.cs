using Geohash;

namespace Location_Service.DataAccesses.Locations.InMemory;

// https://github.com/dice89/proximityhash/blob/master/proximitypyhash.py
public static class ProximityHash
{
    /// <summary>
    /// Get the list of geo hashed that approximate a circle
    /// </summary>
    /// <param name="latitude">The longitude to get the radius approximation for</param>
    /// <param name="longitude">The latitude to get the radius approximation for</param>
    /// <param name="radius">Radius coverage in meters</param>
    /// <param name="precision">The geo hash precision level</param>
    /// <returns>A list of geo hashes</returns>
    public static List<string> GetGeoHashRadiusApproximation(double latitude,
                                                             double longitude,
                                                             int radius,
                                                             int precision)
    {
        const double x = 0.0d;
        const double y = 0.0d;
        
        var points = new List<(double lat, double lon)>();
        var geoHashes = new List<string>();
        
        double[] gridWith = { 5009400.0, 1252300.0, 156500.0, 39100.0, 4900.0, 1200.0, 152.9, 38.2, 4.8, 1.2, 0.149, 0.0370 };
        double[] gridHeight = { 4992600.0, 624100.0, 156000.0, 19500.0, 4900.0, 609.4, 152.4, 19.0, 4.8, 0.595, 0.149, 0.0199 };

        double height = (gridHeight[precision - 1]) / 2;
        double width = (gridWith[precision - 1]) / 2;

        int latMoves = Convert.ToInt32(Math.Ceiling(radius / height));
        int lonMoves = Convert.ToInt32(Math.Ceiling(radius / width));

        for (int latIndex = 0; latIndex < latMoves; latIndex++)
        {
            double tempLat = y + height * latIndex;

            for (int lonIndex = 0; lonIndex < lonMoves; lonIndex++)
            {
                double tempLon = x + width * lonIndex;

                if (IsInCircle(tempLat, tempLon, y, x, radius))
                {
                    (double xCenter, double yCenter) = GetCentroid(tempLat, tempLon, height, width);
                    
                    (double finalLatitutde, double finalLongitude) latLon;   
                    
                    latLon = ConvertToLatLon(yCenter, xCenter, latitude, longitude);
                    points.Add(latLon);
                    
                    latLon = ConvertToLatLon(-yCenter, xCenter, latitude, longitude);
                    points.Add(latLon);

                    latLon = ConvertToLatLon(yCenter, -xCenter, latitude, longitude);
                    points.Add(latLon);

                    latLon = ConvertToLatLon(-yCenter, -xCenter, latitude, longitude);
                    points.Add(latLon);
                }
            }
        }

        var geoHasher = new Geohasher();
        
        foreach (var point in points)
            geoHashes.Add(geoHasher.Encode(point.lat, point.lon, precision));
        
        return geoHashes
                .Distinct()
                .ToList();
    }
    
    
    /// <summary>
    /// Check if given latitude and longitude are within the circle
    /// </summary>
    /// <param name="latitude">To be checked latitude</param>
    /// <param name="longitude">To be checked longitude</param>
    /// <param name="latCenter">The latitude of the centroid</param>
    /// <param name="lonCenter">The longitude of the centroid</param>
    /// <param name="radius">Desired radius in meters</param>
    private static bool IsInCircle(double latitude, 
                                   double longitude, 
                                   double latCenter, 
                                   double lonCenter, 
                                   int radius)
    {
        double xDiff = longitude - lonCenter;
        double yDiff = latitude - latCenter;

        return Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2) <= Math.Pow(radius, 2);
    }

    /// <summary>
    /// Get the centroid of a bounding box
    /// </summary>
    /// <returns>Tuple containing the centroid</returns>
    private static (double xCenter, double yCenter) GetCentroid(double latitude,
                                                                double longitude,
                                                                double height,
                                                                double width)
    {
        double yCenter = latitude + (height / 2);
        double xCenter = longitude + (width / 2);

        return (xCenter, yCenter);
    }

    /// <summary>
    /// Convert the latitude and longitude relative to the centroid
    /// </summary>
    /// <param name="y">y of the centroid</param>
    /// <param name="x">x of the centroid</param>
    /// <param name="lat">latitude</param>
    /// <param name="lon">longitude</param>
    private static (double finalLatitutde, double finalLongitude) ConvertToLatLon(double y,
                                                                                  double x,
                                                                                  double lat,
                                                                                  double lon)
    {
        const double rEarth = 6371000;

        double latDiff = (y / rEarth) * (180 / Math.PI);
        double lonDiff = (x / rEarth) * (180 / Math.PI) / Math.Cos(lat * Math.PI / 180);

        double finalLatitude = lat + latDiff;
        double finalLongitude = lon + lonDiff;

        return (finalLatitude, finalLongitude);
    }
}