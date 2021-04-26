using identityServerNew.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace identityServerNew
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            _config = config;
        }
        public  static IConfiguration _config { get; private set; }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = _config.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(config => {
                //config.UseInMemoryDatabase("Memory");
                config.UseSqlServer(connectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit =false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false ;
                config.SignIn.RequireConfirmedEmail = true;
                config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                config.Lockout.MaxFailedAccessAttempts = 4;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(config => {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
                config.AccessDeniedPath = "/Auth/UserAccessDenied";
            });
            var assembly = typeof(Startup).Assembly.GetName().Name;
            //exei mesa authentication/authorization..
            services.AddIdentityServer()
                //to paketo identity server 4 asp 
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                         sql => sql.MigrationsAssembly(assembly));
                 })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
                //.AddInMemoryApiResources(Configuration.GetApis())
                //.AddInMemoryIdentityResources(Configuration.GetIdentityResources())
                //.AddInMemoryClients(Configuration.GetClients())
                .AddDeveloperSigningCredential();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddAuthentication().AddFacebook(config=>
            {
                config.AppId = _config.GetValue<string>("Facebook:AppId");
                config.AppSecret = _config.GetValue<string>("Facebook:AppSecret");
            });
            services.AddAuthentication().AddGitHub(config =>
            {
                config.ClientId = _config.GetValue<string>("GitHub:ClientId");
                config.ClientSecret = _config.GetValue<string>("GitHub:ClientSecret");
                config.Scope.Add("user:email");

            });

            services.AddAuthorization(config => {
                config.AddPolicy("Admin", policyBuilder => policyBuilder.RequireClaim(ClaimTypes.Role, "Admin"));
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseCors("MyPolicy");

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
