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