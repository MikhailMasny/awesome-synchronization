using Coravel;
using Masny.Application.Extensions;
using Masny.Infrastructure.Extensions;
using Masny.Worker.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace Masny.Worker
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");

                IHost host = CreateHostBuilder(args).Build();
                host.Services.UseScheduler(scheduler =>
                {
                    //scheduler
                    //    .Schedule<PersonSynchronizationTask>()
                    //    .DailyAt(20, 09)
                    //    .Zoned(TimeZoneInfo.Local);

                    //scheduler
                    //    .Schedule<PostSynchronizationTask>()
                    //    .DailyAt(20, 10)
                    //    .Zoned(TimeZoneInfo.Local);

                    scheduler
                        .Schedule<CommentSynchronizationTask>()
                        .DailyAt(21, 04)
                        .Zoned(TimeZoneInfo.Local);
                });
                host.Run();

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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureServices(services =>
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

                    services.AddScheduler();
                    services.AddTransient<PersonSynchronizationTask>();
                    services.AddTransient<PostSynchronizationTask>();
                    services.AddTransient<CommentSynchronizationTask>();
                    services.AddApplication();
                    services.AddInfrastructure(configuration);
                });
    };
}

