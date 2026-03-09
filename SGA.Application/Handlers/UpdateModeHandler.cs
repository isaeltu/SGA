using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class UpdateModeHandler : IRequestHandler<UpdateModeCommand>
    {
        private readonly IModeRepository _modeRepository;

        public UpdateModeHandler(IModeRepository modeRepository)
        {
            _modeRepository = modeRepository;
        }

        public async Task Handle(UpdateModeCommand request, CancellationToken cancellationToken)
        {
            var mode = await _modeRepository.GetByIdAsync(request.ModeId, cancellationToken).ConfigureAwait(false);
            if (mode is null)
            {
                throw new KeyNotFoundException($"Mode with id {request.ModeId} was not found.");
            }

            mode.Rename(request.Name, request.ModifiedBy);
            await _modeRepository.UpdateAsync(mode, cancellationToken).ConfigureAwait(false);
        }
    }
}
