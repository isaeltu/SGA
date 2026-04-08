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

            var query = drivers.Where(d => !d.IsDeleted);
            if (request.OnlyAvailable)
            {
                query = query.Where(d => d.IsAvailable);
            }

            var results = new List<DriverLookupDto>();

            foreach (var driver in query)
            {
                try
                    {
                    var person = await _personRepository.GetByIdAsync(driver.PersonId, cancellationToken).ConfigureAwait(false);
                    if (person is null || person.IsDeleted)
                    {
                        continue;
                    }

                    if (request.InstitutionId.HasValue && person.InstitutionId != request.InstitutionId.Value)
                    {
                        continue;
                    }

                    var fullName = $"{person.FirstName} {person.LastName}".Trim();
                    results.Add(new DriverLookupDto(
                        driver.Id,
                        driver.PersonId,
                        person.InstitutionId,
                        string.IsNullOrWhiteSpace(fullName) ? $"Driver {driver.Id}" : fullName,
                        driver.DriverLicence,
                        driver.IsAvailable));
                }
                catch
                {
                    // Keep list usable even if one specific person record cannot be materialized.
                    if (!request.InstitutionId.HasValue)
                    {
                        results.Add(new DriverLookupDto(
                            driver.Id,
                            driver.PersonId,
                            0,
                            $"Driver {driver.Id}",
                            driver.DriverLicence,
                            driver.IsAvailable));
                    }
                }
            }

            return results
                .OrderBy(x => x.FullName)
                .ThenBy(x => x.DriverId)
                .ToArray();
        }
    }
}
