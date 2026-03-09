using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateInstitutionHandler : IRequestHandler<CreateInstitutionCommand, int>
    {
        private readonly IInstitutionRepository _institutionRepository;

        public CreateInstitutionHandler(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public async Task<int> Handle(CreateInstitutionCommand request, CancellationToken cancellationToken)
        {
            var existing = await _institutionRepository.GetByCodeAsync(request.Code, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                return existing.Id;
            }

            var institution = new Institution(request.Code, request.Name, request.CreatedBy);
            await _institutionRepository.AddAsync(institution, cancellationToken).ConfigureAwait(false);
            return institution.Id;
        }
    }
}