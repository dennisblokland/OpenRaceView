using MediatR;
using OpenRaceView.Application.DTOs;

namespace OpenRaceView.Application.Commands.Laps;

public class CreateLapCommand : IRequest<CreateLapResponse>
{
    public CreateLapRequest Request { get; set; } = new();

    public CreateLapCommand(CreateLapRequest request)
    {
        Request = request;
    }
}