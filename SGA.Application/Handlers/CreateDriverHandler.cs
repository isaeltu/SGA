using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateDriverHandler : IRequestHandler<CreateDriverCommand, int>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IPersonRepository _personRepository;

        public CreateDriverHandler(IDriverRepository driverRepository, IPersonRepository personRepository)
        {
            _driverRepository = driverRepository;
            _personRepository = personRepository;
        }

        public async Task<int> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            var personLookup = await _personRepository.GetDriverLookupByPersonIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (personLookup is null || personLookup.IsDeleted)
            {
                throw new KeyNotFoundException($"Person with id {request.PersonId} was not found.");
            }

            var existingDriver = await _driverRepository.GetByPersonIdAsync(request.PersonId, cancellationToken).ConfigureAwait(false);
            if (existingDriver is not null)
            {
                return existingDriver.Id;
            }

            var driver = new Driver(request.PersonId, request.DriverLicense, request.LicenseExpirationDate)
            {
                PersonId = request.PersonId
            };

            driver.SetCreationInfo(request.CreatedBy);
            await _driverRepository.AddAsync(driver, cancellationToken).ConfigureAwait(false);
            return driver.Id;
        }
    }
}
