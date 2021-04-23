using ApiOne.AuthorizationRequirements;
using ApiOne.Databases;
using ApiOne.Hubs;
using ApiOne.Interfaces;
using ApiOne.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static ApiOne.AuthorizationRequirements.CustomRequireClaim;

namespace ApiOne
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
                config.Audience = "ApiOne";

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

                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role,"Admin"));

                config.AddPolicy("Claim.DoB", policyBuilder => {
                    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                });
                services.AddTransient<IAdRepository, AdRepository>();

            });

            services.AddScoped<IAuthorizationHandler,CustomRequireClaimHandler>();

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {   
                app.UseDeveloperExceptionPage();
            }

            app.UseFileServer(new FileServerOptions { 
            FileProvider= new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Images")),
                RequestPath= "/Images",
                EnableDirectoryBrowsing=true
            });;

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
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapHub<NotificationHub>("/notificationHub");
                //endpoints.MapHub<ChatHub>("/AdminHub");
            });
        }
    }
}
