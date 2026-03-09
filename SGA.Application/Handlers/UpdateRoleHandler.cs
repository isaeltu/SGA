using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.RoleId, cancellationToken).ConfigureAwait(false);
            if (role is null)
            {
                throw new KeyNotFoundException($"Role with id {request.RoleId} was not found.");
            }

            role.UpdateDetails(request.Name, request.Description, request.ModifiedBy);
            await _roleRepository.UpdateAsync(role, cancellationToken).ConfigureAwait(false);
        }
    }
}
