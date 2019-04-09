using System;
using System.Collections.Generic;
using System.Linq;
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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_conf)
                .AddLogging()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(ac =>
                {
                    ac.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "sp.auth",
                        
                        ValidateLifetime = true,
                        
                        IssuerSigningKey = new JsonWebKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            services
                .AddDbContext<AuthDataContext>(db => db.UseInMemoryDatabase("sp.auth"))
                .AddSpAuthServices(_conf)
                .AddCors(
                    co => co.AddPolicy(
                        "AllowAll",
                        p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                    )
                )
                .AddMvc(opt => opt.Filters.Add<CustomExceptionFilterAttribute>())
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SpAuthConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            #if DEBUG
            loggerFactory.AddConsole(LogLevel.Debug);
            #endif

            app.UseAuthentication();
            app.UseCors();
            app.UseMvc(routes => routes.MapRoute(
                name:"default",
                template:"api/{controller=Auth}/{action=Index}/{id?}"
            ));
        }
    }
}