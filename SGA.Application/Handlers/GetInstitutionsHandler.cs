using MediatR;
using SGA.Application.DTOs.Users;
using SGA.Application.Queries;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class GetInstitutionsHandler : IRequestHandler<GetInstitutionsQuery, IReadOnlyCollection<InstitutionDto>>
    {
        private readonly IInstitutionRepository _institutionRepository;

        public GetInstitutionsHandler(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public async Task<IReadOnlyCollection<InstitutionDto>> Handle(GetInstitutionsQuery request, CancellationToken cancellationToken)
        {
            var institutions = await _institutionRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            return institutions
                .Where(i => !i.IsDeleted)
                .Select(i => new InstitutionDto(i.Id, i.Code, i.Name, i.IsActive))
                .OrderBy(i => i.Name)
                .ToArray();
        }
    }
}