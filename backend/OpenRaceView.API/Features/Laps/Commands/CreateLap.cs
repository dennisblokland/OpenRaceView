using FastEndpoints;
using Microsoft.Extensions.Options;
using OpenRaceView.API.Configuration;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Domain.Entities;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.API.Features.Laps.Commands;

public class CreateLap : Endpoint<CreateLapRequest, CreateLapResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly TelemetryOptions _telemetryOptions;

    public CreateLap(ApplicationDbContext context, IOptions<TelemetryOptions> telemetryOptions)
    {
        _context = context;
        _telemetryOptions = telemetryOptions.Value;
    }

    public override void Configure()
    {
        Post("/api/laps");
        AllowAnonymous();
        Description(x => x
            .WithName("CreateLap")
            .WithSummary("Creates a new lap with telemetry data")
            .WithDescription("Creates a new lap record with associated telemetry samples")
            .ProducesValidationProblem()
            .Produces<CreateLapResponse>(201)
            .Produces(400));
    }

    public override async Task HandleAsync(CreateLapRequest req, CancellationToken ct)
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(req.TrackName))
            ThrowError("Track name is required");

        if (string.IsNullOrWhiteSpace(req.Source))
            ThrowError("Source is required");

        if (req.DurationMs <= 0)
            ThrowError("Duration must be positive");

        if (!req.Samples.Any())
            ThrowError("At least one sample is required");

        if (req.Samples.Count > _telemetryOptions.MaxSamplesPerLap)
            ThrowError($"Cannot exceed {_telemetryOptions.MaxSamplesPerLap} samples per lap");

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
        await _context.SaveChangesAsync(ct);

        // Set location header and return 201 Created
        await Send.OkAsync(new CreateLapResponse { Id = lap.Id }, ct);
   
        HttpContext.Response.Headers.Location = $"/api/laps/{lap.Id}";
    }
}