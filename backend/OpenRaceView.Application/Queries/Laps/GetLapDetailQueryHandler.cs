using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.Application.Queries.Laps;

public class GetLapDetailQueryHandler : IRequestHandler<GetLapDetailQuery, LapDetailDto?>
{
    private readonly ApplicationDbContext _context;

    public GetLapDetailQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LapDetailDto?> Handle(GetLapDetailQuery request, CancellationToken cancellationToken)
    {
        IQueryable<OpenRaceView.Domain.Entities.Lap> query = _context.Laps.AsQueryable();

        if (request.IncludeSamples)
        {
            query = query.Include(l => l.Samples.OrderBy(s => s.Index));
        }

        OpenRaceView.Domain.Entities.Lap? lap = await query.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

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