using MediatR;

namespace SGA.Application.Commands
{
    public sealed record DeleteBusCommand(int BusId) : IRequest;
}