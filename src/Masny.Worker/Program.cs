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
            const string hostStart = "Starting web host.";
            const string hostExceptrion = "Host terminated unexpectedly";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information(hostStart);

                IHost host = CreateHostBuilder(args).Build();
                host.Services.UseScheduler(scheduler =>
                {
                    scheduler
                        .Schedule<PersonSynchronizationTask>()
                        .DailyAt(05, 00)
                        .Zoned(TimeZoneInfo.Utc);

                    scheduler
                        .Schedule<PostSynchronizationTask>()
                        .DailyAt(05, 01)
                        .Zoned(TimeZoneInfo.Utc);

                    scheduler
                        .Schedule<CommentSynchronizationTask>()
                        .DailyAt(05, 02)
                        .Zoned(TimeZoneInfo.Utc);
                });
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, hostExceptrion);
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

