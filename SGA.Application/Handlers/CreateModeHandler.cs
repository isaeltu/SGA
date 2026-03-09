using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateModeHandler : IRequestHandler<CreateModeCommand, int>
    {
        private readonly IModeRepository _modeRepository;

        public CreateModeHandler(IModeRepository modeRepository)
        {
            _modeRepository = modeRepository;
        }

        public async Task<int> Handle(CreateModeCommand request, CancellationToken cancellationToken)
        {
            var existing = await _modeRepository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                return existing.Id;
            }

            var mode = new Mode(request.Name, request.CreatedBy);
            await _modeRepository.AddAsync(mode, cancellationToken).ConfigureAwait(false);
            return mode.Id;
        }
    }
}