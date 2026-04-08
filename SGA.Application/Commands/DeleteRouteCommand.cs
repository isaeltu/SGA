using MediatR;

namespace SGA.Application.Commands
{
    public sealed record DeleteRouteCommand(int RouteId) : IRequest;
}
