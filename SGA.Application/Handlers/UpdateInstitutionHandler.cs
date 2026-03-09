using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class UpdateInstitutionHandler : IRequestHandler<UpdateInstitutionCommand>
    {
        private readonly IInstitutionRepository _institutionRepository;

        public UpdateInstitutionHandler(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public async Task Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
        {
            var institution = await _institutionRepository.GetByIdAsync(request.InstitutionId, cancellationToken).ConfigureAwait(false);
            if (institution is null)
            {
                throw new KeyNotFoundException($"Institution with id {request.InstitutionId} was not found.");
            }

            institution.UpdateDetails(request.Code, request.Name, request.ModifiedBy);

            if (request.IsActive)
            {
                institution.Activate(request.ModifiedBy);
            }
            else
            {
                institution.Deactivate(request.ModifiedBy);
            }

            await _institutionRepository.UpdateAsync(institution, cancellationToken).ConfigureAwait(false);
        }
    }
}
