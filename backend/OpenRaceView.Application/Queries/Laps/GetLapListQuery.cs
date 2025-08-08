using MediatR;
using OpenRaceView.Application.DTOs;

namespace OpenRaceView.Application.Queries.Laps;

public class GetLapListQuery : IRequest<List<LapListItemDto>>
{
}