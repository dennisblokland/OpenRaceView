namespace OpenRaceView.Application.DTOs;

public class CreateLapRequest
{
    public string Source { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime StartTimeUtc { get; set; }
    public int DurationMs { get; set; }
    public double? DistanceMeters { get; set; }
    public List<CreateLapSampleDto> Samples { get; set; } = new();
}

public class CreateLapSampleDto
{
    public int T { get; set; } // Timestamp offset in ms
    public double Lat { get; set; }
    public double Lon { get; set; }
    public double? Elev { get; set; }
    public double? Spd { get; set; }
    public double? Throttle { get; set; }
    public double? Brake { get; set; }
}

public class LapListItemDto
{
    public Guid Id { get; set; }
    public string TrackName { get; set; } = string.Empty;
    public DateTime StartTimeUtc { get; set; }
    public int DurationMs { get; set; }
    public int SampleCount { get; set; }
    public DateTime CreatedUtc { get; set; }
}

public class LapDetailDto
{
    public Guid Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime StartTimeUtc { get; set; }
    public int DurationMs { get; set; }
    public double? DistanceMeters { get; set; }
    public int SampleCount { get; set; }
    public DateTime CreatedUtc { get; set; }
    public List<LapSampleDto>? Samples { get; set; }
}

public class LapSampleDto
{
    public int Index { get; set; }
    public int TimestampOffsetMs { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? ElevationM { get; set; }
    public double? SpeedMps { get; set; }
    public double? ThrottlePct { get; set; }
    public double? BrakePct { get; set; }
}

public class CreateLapResponse
{
    public Guid Id { get; set; }
}