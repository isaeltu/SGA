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
            Dictionary<int, (int InstitutionId, string FullName)>? personById = null;

            try
            {
                var people = await _personRepository.GetAllAsync(cancellationToken).ConfigureAwait(false);
                personById = people
                    .Where(p => !p.IsDeleted)
                    .ToDictionary(
                        p => p.Id,
                        p =>
                        {
                            var fullName = $"{p.FirstName} {p.LastName}".Trim();
                            return (p.InstitutionId, string.IsNullOrWhiteSpace(fullName) ? $"Driver {p.Id}" : fullName);
                        });
            }
            catch
            {
                // Some legacy records can fail full Person materialization.
                // Keep endpoint usable by returning driver info without person details.
                personById = null;
            }

            var query = drivers.Where(d => !d.IsDeleted);
            if (request.OnlyAvailable)
            {
                query = query.Where(d => d.IsAvailable);
            }

            IEnumerable<DriverLookupDto> results;

            if (personById is null)
            {
                results = query.Select(d => new DriverLookupDto(
                    d.Id,
                    d.PersonId,
                    0,
                    $"Driver {d.Id}",
                    d.DriverLicence,
                    d.IsAvailable));
            }
            else
            {
                results = query
                    .Where(d => personById.ContainsKey(d.PersonId))
                    .Select(d =>
                    {
                        var person = personById[d.PersonId];
                        return new DriverLookupDto(
                            d.Id,
                            d.PersonId,
                            person.InstitutionId,
                            person.FullName,
                            d.DriverLicence,
                            d.IsAvailable);
                    });

                if (request.InstitutionId.HasValue)
                {
                    results = results.Where(x => x.InstitutionId == request.InstitutionId.Value);
                }
            }

            return results
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.DriverId)
                .ToArray();
        }
    }
}
