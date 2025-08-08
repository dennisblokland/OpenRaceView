using MediatR;
using OpenRaceView.Application.DTOs;

namespace OpenRaceView.Application.Queries.Laps;

public class GetLapDetailQuery : IRequest<LapDetailDto?>
{
    public Guid Id { get; set; }
    public bool IncludeSamples { get; set; }

    public GetLapDetailQuery(Guid id, bool includeSamples = false)
    {
        Id = id;
        IncludeSamples = includeSamples;
    }
}