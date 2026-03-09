using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGA.Application.Behaviors;
using SGA.Application.Commands;
using SGA.Application.Validations;
using System.Reflection;

namespace SGA.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
           this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IValidator<UpdateStudentCommand>, UpdateStudentCommandValidator>();

            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
            
        }

        
        
        
            
    }
}
