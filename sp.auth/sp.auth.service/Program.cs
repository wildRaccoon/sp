﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace sp.auth.service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging( lb =>
                        lb.AddConsole()
                            .AddDebug()
                            .SetMinimumLevel(LogLevel.None)
                    )
                .UseStartup<Startup>();
    }
}