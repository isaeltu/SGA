using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class UpdatePermissionHandler : IRequestHandler<UpdatePermissionCommand>
    {
        private readonly IPermissionRepository _permissionRepository;

        public UpdatePermissionHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _permissionRepository.GetByIdAsync(request.PermissionId, cancellationToken).ConfigureAwait(false);
            if (permission is null)
            {
                throw new KeyNotFoundException($"Permission with id {request.PermissionId} was not found.");
            }

            permission.UpdateDetails(request.Name, request.Description, request.ModifiedBy);
            await _permissionRepository.UpdateAsync(permission, cancellationToken).ConfigureAwait(false);
        }
    }
}
