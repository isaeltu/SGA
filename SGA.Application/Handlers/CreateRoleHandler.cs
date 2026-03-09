using MediatR;
using SGA.Application.Commands;
using SGA.Domain.Entities.Users;
using SGA.Domain.Repositories.Users;

namespace SGA.Application.Handlers
{
    public sealed class CreateRoleHandler : IRequestHandler<CreateRoleCommand, byte>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<byte> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var existing = await _roleRepository.GetByNameAsync(request.Name, cancellationToken).ConfigureAwait(false);
            if (existing is not null)
            {
                return existing.Id;
            }

            var role = new Role(request.Name, request.Description, request.CreatedBy);
            await _roleRepository.AddAsync(role, cancellationToken).ConfigureAwait(false);
            return role.Id;
        }
    }
}