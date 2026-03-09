using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreatePermissionHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IPermissionRepository _permissionRepository;

        public CreatePermissionHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var existing = await _permissionRepository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                return existing.Id;
            }

            var permission = new Permission(request.Name, request.Description, request.CreatedBy);
            await _permissionRepository.AddAsync(permission, cancellationToken).ConfigureAwait(false);
            return permission.Id;
        }
    }
}