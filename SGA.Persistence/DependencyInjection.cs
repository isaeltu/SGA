using Microsoft.Extensions.DependencyInjection;
using SGA.Application.Abstractions.Messaging;
using SGA.Application.Abstractions.MultiTenancy;
using SGA.Domain.Repositories;
using SGA.Domain.Repositories.Base;
using SGA.Domain.Repositories.Users;
using SGA.Domain.Services.Interfaces.MultiTenancy;
using SGA.Domain.Services.MultiTenancy;
using SGA.Persistence.Context;
using SGA.Persistence.MultiTenancy;
using SGA.Persistence.Messaging;
using SGA.Persistence.Repositories;
using SGA.Persistence.Repositories.Trips;
using SGA.Persistence.Repositories.Users;

namespace SGA.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
            services.AddScoped<IBusRepository, BusRepository>();
            services.AddScoped<IIncidentRepository, IncidentRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();

            services.AddScoped<IAdministratorRepository, AdministratorRepository>();
            services.AddScoped<ICollegeRepository, CollegeRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IModeRepository, ModeRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<ITenantContext, TenantContext>();
            services.AddScoped<IMultiTenantDomainService, MultiTenantDomainService>();
            services.AddScoped<IMessageBus, InMemoryMessageBus>();
            services.AddScoped<IDomainEventSerializer, DomainEventSerializer>();
            services.AddScoped<IOutboxProcessor, OutboxProcessor>();

            return services;
        }
    }
}
