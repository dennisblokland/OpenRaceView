using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using OpenRaceView.Application.DTOs;
using OpenRaceView.Infrastructure.Data;

namespace OpenRaceView.API.Features.Laps.Queries;

public class GetLapList : EndpointWithoutRequest<List<LapListItemDto>>
{
    private readonly ApplicationDbContext _context;

    public GetLapList(ApplicationDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/api/laps");
        AllowAnonymous();
        Description(x => x
            .WithName("GetLapList")
            .WithSummary("Gets a list of all laps")
            .WithDescription("Retrieves a list of all laps ordered by start time")
            .Produces<List<LapListItemDto>>(200)
            .Produces(500));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        List<LapListItemDto> result = await _context.Laps
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
            .ToListAsync(ct);

        await Send.OkAsync(result, ct);
    }
}