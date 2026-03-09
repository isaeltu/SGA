using MediatR;
using SGA.Application.DTOs.Users;
using SGA.Application.Queries;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class GetInstitutionByIdHandler : IRequestHandler<GetInstitutionByIdQuery, InstitutionDto?>
    {
        private readonly IInstitutionRepository _institutionRepository;

        public GetInstitutionByIdHandler(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public async Task<InstitutionDto?> Handle(GetInstitutionByIdQuery request, CancellationToken cancellationToken)
        {
            var institution = await _institutionRepository.GetByIdAsync(request.InstitutionId, cancellationToken).ConfigureAwait(false);
            if (institution is null)
            {
                return null;
            }

            return new InstitutionDto(institution.Id, institution.Code, institution.Name, institution.IsActive);
        }
    }
}
