using SGA.Web.Components;
using SGA.Web.Models;
using SGA.Web.Services;
using System.Net.Http;

namespace SGA.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Conecction string 

           


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));
            builder.Services.AddHttpClient<SgaApiClient>((services, client) =>
            {
                var settings = services.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            })
            .ConfigurePrimaryHttpMessageHandler(services =>
            {
                var settings = services.GetRequiredService<Microsoft.Extensions.Options.IOptions<ApiSettings>>().Value;
                var env = services.GetRequiredService<IHostEnvironment>();

                if (!env.IsDevelopment())
                {
                    return new HttpClientHandler();
                }

                if (!Uri.TryCreate(settings.BaseUrl, UriKind.Absolute, out var apiUri))
                {
                    return new HttpClientHandler();
                }

                if (apiUri.Scheme == Uri.UriSchemeHttps &&
                    (string.Equals(apiUri.Host, "localhost", StringComparison.OrdinalIgnoreCase) || apiUri.Host == "127.0.0.1"))
                {
                    return new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                }

                return new HttpClientHandler();
            });
            builder.Services.AddScoped<PortalSessionService>();
            builder.Services.AddScoped<PortalSessionStorageService>();
            builder.Services.AddScoped<UiFeedbackService>();
            builder.Services.AddScoped<UiErrorHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
