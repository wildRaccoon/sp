using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using sp.auth.app.infra.config;
using sp.auth.persistence;

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
                .AddSpAuthServices(_conf)
                .AddCors(
                    co => co.AddPolicy(
                        "AllowAll", 
                        p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials()
                        )
                    )
                .AddMvc();
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

            app.UseCors();
            app.UseMvc(routes => routes.MapRoute(
                name:"default",
                template:"api/{controller=Auth}/{action=Index}/{id?}"
            ));
        }
    }
}