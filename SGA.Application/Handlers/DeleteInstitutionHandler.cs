using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class DeleteInstitutionHandler : IRequestHandler<DeleteInstitutionCommand>
    {
        private readonly IInstitutionRepository _institutionRepository;

        public DeleteInstitutionHandler(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public async Task Handle(DeleteInstitutionCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _institutionRepository.DeleteAsync(request.InstitutionId, cancellationToken).ConfigureAwait(false);
            if (!deleted)
            {
                throw new KeyNotFoundException($"Institution with id {request.InstitutionId} was not found.");
            }
        }
    }
}
