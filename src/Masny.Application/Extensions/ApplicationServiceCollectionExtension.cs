using AutoMapper;
using Masny.Application.Interfaces;
using Masny.Application.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Masny.Application.Extensions
{
    /// <summary>
    /// Service collection for Application project.
    /// </summary>
    public static class ApplicationServiceCollectionExtension
    {
        /// <summary>
        /// Dependency injection.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IPersonManager, PersonManager>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ICloudManager, CloudManager>();

            return services;
        }
    }
}
