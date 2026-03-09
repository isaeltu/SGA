using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SGA.Persistence;
using SGA.Persistence.Context;
using SGA.Domain.Repositories.Base;
using SGA.Persistence.Repositories.SGA.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SGA.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services
            )
        {
            services.AddScoped<ApplicationDbContext>(sp => 
            sp.GetRequiredService<ApplicationDbContext>());

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            return services;
        }
    }
}
