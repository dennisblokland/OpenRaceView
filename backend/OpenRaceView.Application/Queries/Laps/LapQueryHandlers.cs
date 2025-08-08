using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.Application.Queries.Laps;

public class GetLapListQueryHandler : IRequestHandler<GetLapListQuery, List<LapListItemDto>>
{
    private readonly ApplicationDbContext _context;

    public GetLapListQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LapListItemDto>> Handle(GetLapListQuery request, CancellationToken cancellationToken)
    {
        return await _context.Laps
            .OrderByDescending(l => l.StartTimeUtc)
            .Select(l => new LapListItemDto
            {
                Id = l.Id,
                TrackName = l.TrackName,
                StartTimeUtc = l.StartTimeUtc,
                DurationMs = l.DurationMs,
                SampleCount = l.SampleCount,
                CreatedUtc = l.CreatedUtc
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetLapDetailQueryHandler : IRequestHandler<GetLapDetailQuery, LapDetailDto?>
{
    private readonly ApplicationDbContext _context;

    public GetLapDetailQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LapDetailDto?> Handle(GetLapDetailQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Laps.AsQueryable();

        if (request.IncludeSamples)
        {
            query = query.Include(l => l.Samples.OrderBy(s => s.Index));
        }

        var lap = await query.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (lap == null)
            return null;

        return new LapDetailDto
        {
            Id = lap.Id,
            Source = lap.Source,
            TrackName = lap.TrackName,
            StartTimeUtc = lap.StartTimeUtc,
            DurationMs = lap.DurationMs,
            DistanceMeters = lap.DistanceMeters,
            SampleCount = lap.SampleCount,
            CreatedUtc = lap.CreatedUtc,
            Samples = request.IncludeSamples 
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
    }
}