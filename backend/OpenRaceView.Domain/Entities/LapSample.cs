namespace OpenRaceView.Domain.Entities;

public class LapSample
{
    public long Id { get; set; }
    public Guid LapId { get; set; }
    public int Index { get; set; }
    public int TimestampOffsetMs { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? ElevationM { get; set; }
    public double? SpeedMps { get; set; }
    public double? ThrottlePct { get; set; }
    public double? BrakePct { get; set; }

    public Lap Lap { get; set; } = null!;

    public static LapSample Create(
        int timestampOffsetMs,
        double latitude,
        double longitude,
        double? elevationM = null,
        double? speedMps = null,
        double? throttlePct = null,
        double? brakePct = null)
    {
        // Validate latitude and longitude bounds
        if (latitude < -90 || latitude > 90)
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180");
        
        if (timestampOffsetMs < 0)
            throw new ArgumentOutOfRangeException(nameof(timestampOffsetMs), "Timestamp offset must be non-negative");

        // Validate percentages
        if (throttlePct.HasValue && (throttlePct < 0 || throttlePct > 100))
            throw new ArgumentOutOfRangeException(nameof(throttlePct), "Throttle percentage must be between 0 and 100");
        
        if (brakePct.HasValue && (brakePct < 0 || brakePct > 100))
            throw new ArgumentOutOfRangeException(nameof(brakePct), "Brake percentage must be between 0 and 100");

        return new LapSample
        {
            TimestampOffsetMs = timestampOffsetMs,
            Latitude = latitude,
            Longitude = longitude,
            ElevationM = elevationM,
            SpeedMps = speedMps,
            ThrottlePct = throttlePct,
            BrakePct = brakePct
        };
    }
}