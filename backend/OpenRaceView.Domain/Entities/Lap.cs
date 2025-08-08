namespace OpenRaceView.Domain.Entities;

public class Lap
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Source { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string TrackName { get; set; } = string.Empty;
    public DateTime StartTimeUtc { get; set; }
    public int DurationMs { get; set; }
    public double? DistanceMeters { get; set; }
    public int SampleCount { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public List<LapSample> Samples { get; set; } = new();

    public static Lap Create(
        string source,
        string trackName,
        DateTime startTimeUtc,
        int durationMs,
        double? distanceMeters,
        Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("Source cannot be empty", nameof(source));
        
        if (string.IsNullOrWhiteSpace(trackName))
            throw new ArgumentException("Track name cannot be empty", nameof(trackName));
        
        if (trackName.Length > 200)
            throw new ArgumentException("Track name cannot exceed 200 characters", nameof(trackName));
        
        if (durationMs <= 0)
            throw new ArgumentException("Duration must be positive", nameof(durationMs));

        return new Lap
        {
            Source = source,
            TrackName = trackName,
            StartTimeUtc = startTimeUtc,
            DurationMs = durationMs,
            DistanceMeters = distanceMeters,
            UserId = userId
        };
    }

    public void AddSamples(IEnumerable<LapSample> samples, int maxSamplesPerLap)
    {
        var sampleList = samples.ToList();
        
        if (!sampleList.Any())
            throw new ArgumentException("Samples cannot be empty");
        
        if (sampleList.Count > maxSamplesPerLap)
            throw new ArgumentException($"Cannot exceed {maxSamplesPerLap} samples per lap");

        // Validate samples are ordered by timestamp
        for (int i = 1; i < sampleList.Count; i++)
        {
            if (sampleList[i].TimestampOffsetMs < sampleList[i - 1].TimestampOffsetMs)
                throw new ArgumentException("Samples must be ordered by timestamp offset");
        }

        // Validate all timestamps are non-negative
        if (sampleList.Any(s => s.TimestampOffsetMs < 0))
            throw new ArgumentException("All timestamp offsets must be non-negative");

        // Set lap reference and index for each sample
        for (int i = 0; i < sampleList.Count; i++)
        {
            sampleList[i].LapId = Id;
            sampleList[i].Index = i;
        }

        Samples.AddRange(sampleList);
        SampleCount = Samples.Count;
    }
}