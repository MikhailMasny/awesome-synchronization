using Masny.Application.Interfaces;
using Masny.Infrastructure.AppContext;
using Masny.Infrastructure.CloudContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Masny.Infrastructure.Extensions
{
    /// <summary>
    /// Service collection for Infrastructure project.
    /// </summary>
    public static class InfrastructureServiceCollectionExtension
    {
        /// <summary>
        /// Dependency injection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppConnection")));
            services.AddDbContext<CloudDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CloudConnection")));

            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
            services.AddScoped<ICloudDbContext>(provider => provider.GetService<CloudDbContext>());

            return services;
        }
    }
}
