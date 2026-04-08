using MediatR;
using SGA.Application.DTOs.Users;
using SGA.Application.Queries;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class GetDriversHandler : IRequestHandler<GetDriversQuery, IReadOnlyCollection<DriverLookupDto>>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IPersonRepository _personRepository;

        public GetDriversHandler(IDriverRepository driverRepository, IPersonRepository personRepository)
        {
            _driverRepository = driverRepository;
            _personRepository = personRepository;
        }

        public async Task<IReadOnlyCollection<DriverLookupDto>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
        {
            var drivers = await _driverRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var people = await _personRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            var personById = people
                .Where(p => !p.IsDeleted)
                .ToDictionary(p => p.Id);

            var query = drivers.Where(d => !d.IsDeleted);
            if (request.OnlyAvailable)
            {
                query = query.Where(d => d.IsAvailable);
            }

            var results = query
                .Where(d => personById.ContainsKey(d.PersonId))
                .Select(d =>
                {
                    var person = personById[d.PersonId];
                    var fullName = $"{person.FirstName} {person.LastName}".Trim();

                    return new DriverLookupDto(
                        d.Id,
                        d.PersonId,
                        person.InstitutionId,
                        string.IsNullOrWhiteSpace(fullName) ? $"Driver {d.Id}" : fullName,
                        d.DriverLicence,
                        d.IsAvailable);
                });

            if (request.InstitutionId.HasValue)
            {
                results = results.Where(x => x.InstitutionId == request.InstitutionId.Value);
            }

            return results
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.DriverId)
                .ToArray();
        }
    }
}
