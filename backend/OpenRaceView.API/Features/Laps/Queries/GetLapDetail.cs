using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.API.Features.Laps.Queries;

public class GetLapDetailRequest
{
    public Guid Id { get; set; }
    public bool IncludeSamples { get; set; }
}

public class GetLapDetail : Endpoint<GetLapDetailRequest, LapDetailDto>
{
    private readonly ApplicationDbContext _context;

    public GetLapDetail(ApplicationDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/api/laps/{id}");
        AllowAnonymous();
        Description(x => x
            .WithName("GetLapDetail")
            .WithSummary("Gets detailed information about a specific lap")
            .WithDescription("Retrieves detailed information about a lap, optionally including telemetry samples")
            .Produces<LapDetailDto>(200)
            .Produces(404)
            .Produces(500));
    }

    public override async Task HandleAsync(GetLapDetailRequest req, CancellationToken ct)
    {
        IQueryable<OpenRaceView.Domain.Entities.Lap> query = _context.Laps.AsQueryable();

        if (req.IncludeSamples)
        {
            query = query.Include(l => l.Samples.OrderBy(s => s.Index));
        }

        OpenRaceView.Domain.Entities.Lap? lap = await query.FirstOrDefaultAsync(l => l.Id == req.Id, ct);

        if (lap == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        LapDetailDto result = new LapDetailDto
        {
            Id = lap.Id,
            Source = lap.Source,
            TrackName = lap.TrackName,
            StartTimeUtc = lap.StartTimeUtc,
            DurationMs = lap.DurationMs,
            DistanceMeters = lap.DistanceMeters,
            SampleCount = lap.SampleCount,
            CreatedUtc = lap.CreatedUtc,
            Samples = req.IncludeSamples 
                ? lap.Samples.Select(s => new LapSampleDto
                {
                    Index = s.Index,
                    TimestampOffsetMs = s.TimestampOffsetMs,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    ElevationM = s.ElevationM,
                    SpeedMps = s.SpeedMps,
                    ThrottlePct = s.ThrottlePct,
                    BrakePct = s.BrakePct
                }).ToList()
                : null
        };

        await Send.OkAsync(result, ct);
    }
}