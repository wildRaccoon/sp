using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace sp.wallet.service
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var maxTimeStamp = DateTimeOffset.FromUnixTimeMilliseconds(253402300799999);

            app.Run(async (context) =>
            {
                var current = DateTimeOffset.UtcNow;
                var maxDate = new DateTime(maxTimeStamp.Ticks,DateTimeKind.Utc).ToLocalTime();
                await context.Response.WriteAsync($"{current.ToUnixTimeMilliseconds()} - {current.ToUnixTimeSeconds()} - {maxDate.ToLongDateString()} {maxDate.ToLongTimeString()}");
            });
        }
    }
}
