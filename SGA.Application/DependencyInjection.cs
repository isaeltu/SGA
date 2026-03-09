using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGA.Application.Commands;
using SGA.Application.Handlers;
using SGA.Application.Validations;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc;

namespace SGA.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
           this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<UpdateStudentHandler>();

            services.AddScoped<IValidator<UpdateStudentCommand>, UpdateStudentCommandValidator>();

            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
            
        }

        
        
        
            
    }
}
