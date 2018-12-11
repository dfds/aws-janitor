﻿using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace IAMRoleService.WebApi
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Information()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                         .Enrich.FromLogContext()
                         .WriteTo.Console(new CompactJsonFormatter())
                         .CreateLogger();

            try
            {
                Log.Information("Starting host");
                CreateWebHostBuilder(args)
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseSerilog()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
    }
}