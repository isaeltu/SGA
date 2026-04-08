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
                    var personLookup = await _personRepository.GetDriverLookupByPersonIdAsync(driver.PersonId, cancellationToken).ConfigureAwait(false);
                    if (personLookup is null || personLookup.IsDeleted)
                    {
                        continue;
                    }

                    if (request.InstitutionId.HasValue && personLookup.InstitutionId != request.InstitutionId.Value)
                    {
                        continue;
                    }

                    results.Add(new DriverLookupDto(
                        driver.Id,
                        driver.PersonId,
                        personLookup.InstitutionId,
                        personLookup.FullName,
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
