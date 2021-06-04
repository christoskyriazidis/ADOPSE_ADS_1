using DypaApi.Hubs;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangfire;
using Hangfire.MemoryStorage;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Diagnostics;
using DypaApi.CronJobs;

namespace DypaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            StaticConfig = configuration;
        }
        public static IConfiguration StaticConfig { get; private set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", config => {
                config.Authority = "https://localhost:44305/";
                //kai kala poios mporei na mpei edw.. (ApiOne)
                config.Audience = "ApiDypa";

                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization(config => {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.Country)
                //.Build()

                //config.DefaultPolicy = defaultAuthPolicy;

                //config.AddPolicy("Claim.DoB", policyBuilder => {
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder => {
                    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });
            });
            services.AddHangfire(config => {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage();
                
            });
            services.AddHangfireServer();
            services.AddControllers()
           .AddNewtonsoftJson(
             options =>
             {
                 options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             }).AddMvcOptions(options =>
             {
                 options.MaxModelValidationErrors = 50;
                 options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                     _ => "The field is required.");
             });
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
            services.AddSingleton<ICronJob, CronJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
               Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath = "/Images",
                EnableDirectoryBrowsing = true
            });

            //app.UseCors("AllowAll");
            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials
            app.UseRouting();

            //who are you
            app.UseAuthentication();

            //are you allowed?
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationHub");   
            });
            app.UseHangfireDashboard();

            recurringJobManager.AddOrUpdate("HourlySensors",
                () => serviceProvider.GetService<ICronJob>().FetchSensorAsync(),
                Cron.Minutely);
            //recurringJobManager.AddOrUpdate("WeeklyForecast",
            //    () => serviceProvider.GetService<ICronJob>().WeeklyForecastAsync(),
            //    Cron.Minutely);

        }
    }
}
