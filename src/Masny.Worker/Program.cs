using Coravel;
using Masny.Application.Extensions;
using Masny.Infrastructure.Extensions;
using Masny.Worker.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Masny.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler
                    .Schedule<PersonSynchronizationTask>()
                    .DailyAt(20, 09)
                    .Zoned(TimeZoneInfo.Local);

                scheduler
                    .Schedule<PostSynchronizationTask>()
                    .DailyAt(20, 10)
                    .Zoned(TimeZoneInfo.Local);

                scheduler
                    .Schedule<CommentSynchronizationTask>()
                    .DailyAt(20, 11)
                    .Zoned(TimeZoneInfo.Local);
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
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

