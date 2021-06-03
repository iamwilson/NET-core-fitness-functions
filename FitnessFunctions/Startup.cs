using System;
using FitnessFunctions.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FitnessFunctions
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddControllers();
            services.AddHealthChecks()
                .AddUrlGroup(new Uri("https://localhost:44366/home"), name: "Home URL", failureStatus: HealthStatus.Degraded, tags: new[]{ "home" })
                .AddUrlGroup(new Uri("https://localhost:44366/api/values"), name: "Values URL", failureStatus: HealthStatus.Degraded, tags: new[] { "values"})
                .AddCheck<CustomHealthCheck>("Custom Health Check", failureStatus: HealthStatus.Unhealthy, tags: new[] { "custome"});

            services.AddHealthChecksUI(opt =>
            {
                opt.AddHealthCheckEndpoint("Main APIs", "/health"); //map health check api 
                opt.MaximumHistoryEntriesPerEndpoint(32); //maximum history of checks    
                opt.SetApiMaxActiveRequests(1); //api requests concurrency    
                opt.SetEvaluationTimeInSeconds(4); //time in seconds between check 
                opt.SetMinimumSecondsBetweenFailureNotifications(8);
            })
            .AddInMemoryStorage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI();
            });
        }
    }
}
