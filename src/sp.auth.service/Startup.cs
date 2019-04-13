using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using sp.auth.app.infra.config;
using sp.auth.persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.infra.ef;
using sp.auth.service.filters;

namespace sp.auth.service
{
    public class Startup
    {
        private readonly IConfiguration _conf;

        public Startup(IHostingEnvironment env)
        {
            _conf = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddUserSecrets(env.EnvironmentName)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json",true)
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var secret = _conf.GetValue<string>("sp.auth:hash:secret");
            var key = Encoding.ASCII.GetBytes(secret);
            
            services.AddSingleton(_conf)
                .AddLogging()
                .AddAuthentication(a =>
                {
                    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(ac =>
                {
                    ac.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "sp.auth",
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        RequireSignedTokens = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false
                    };
                });

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<AuthDataContext>(
                    opt => opt.UseNpgsql(_conf.GetConnectionString("SpAuthDb"))
                )
                .AddSpAuthServices(_conf)
                .AddCors(
                    co => co.AddPolicy(
                        "AllowAll",
                        p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                    )
                )
                .AddHttpContextAccessor()
                .AddMvc(opt => opt.Filters.Add<CustomExceptionFilterAttribute>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SpAuthConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AuthDataContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors();
            app.UseMvc(routes => routes.MapRoute(
                name:"default",
                template:"api/{controller=Auth}/{action=Index}/{id?}"
            ));

            dbContext.Database.EnsureCreated();
        }
    }
}