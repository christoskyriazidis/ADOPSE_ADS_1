using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config=> {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc",config=> {
                    config.Authority = "https://localhost:44305/";
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    //save tokens to cookie
                    config.SaveTokens = true;
                    config.ResponseType = "code";

                    //configure cookie claim mapping


                    config.ClaimActions.MapJsonKey("email", "email", "string");
                    config.ClaimActions.MapJsonKey("username", "username", "string");
                    config.ClaimActions.MapJsonKey("sta8ero", "sta8ero", "string");
                    config.ClaimActions.MapJsonKey("kinito", "kinito", "string");
                    config.ClaimActions.MapJsonKey("test", "test", "string");

                    config.ClaimActions.DeleteClaim("s_hash");
                    //kanei deutero trip
                    config.GetClaimsFromUserInfoEndpoint = true;
                    //configure scope
                    config.Scope.Clear();
                    config.Scope.Add("openid");
                    config.Scope.Add("credentials");
                    config.Scope.Add("ApiOne");
                    config.Scope.Add("offline_access");
                });

            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
