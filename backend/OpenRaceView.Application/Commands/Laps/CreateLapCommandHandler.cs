using MediatR;
using Microsoft.Extensions.Options;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Domain.Entities;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.Application.Commands.Laps;

public class CreateLapCommandHandler : IRequestHandler<CreateLapCommand, CreateLapResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly TelemetryOptions _telemetryOptions;

    public CreateLapCommandHandler(ApplicationDbContext context, IOptions<TelemetryOptions> telemetryOptions)
    {
        _context = context;
        _telemetryOptions = telemetryOptions.Value;
    }

    public async Task<CreateLapResponse> Handle(CreateLapCommand request, CancellationToken cancellationToken)
    {
        CreateLapRequest req = request.Request;

        // Validate request
        if (string.IsNullOrWhiteSpace(req.TrackName))
            throw new ArgumentException("Track name is required");

        if (string.IsNullOrWhiteSpace(req.Source))
            throw new ArgumentException("Source is required");

        if (req.DurationMs <= 0)
            throw new ArgumentException("Duration must be positive");

        if (!req.Samples.Any())
            throw new ArgumentException("At least one sample is required");

        if (req.Samples.Count > _telemetryOptions.MaxSamplesPerLap)
            throw new ArgumentException($"Cannot exceed {_telemetryOptions.MaxSamplesPerLap} samples per lap");

        // Create lap entity
        Lap lap = Lap.Create(
            req.Source,
            req.TrackName,
            req.StartTimeUtc,
            req.DurationMs,
            req.DistanceMeters);

        // Convert and validate samples
        List<LapSample> samples = req.Samples.Select(s => LapSample.Create(
            s.T,
            s.Lat,
            s.Lon,
            s.Elev,
            s.Spd,
            s.Throttle,
            s.Brake)).ToList();

        // Add samples to lap (this validates ordering and constraints)
        lap.AddSamples(samples, _telemetryOptions.MaxSamplesPerLap);

        // Save to database
        _context.Laps.Add(lap);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateLapResponse { Id = lap.Id };
    }
}

public class TelemetryOptions
{
    public int MaxSamplesPerLap { get; set; } = 50000;
}