using SGA.Web.Components;
using SGA.Web.Models;
using SGA.Web.Services;

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
            });
            builder.Services.AddScoped<PortalSessionService>();
            builder.Services.AddScoped<UiFeedbackService>();

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
