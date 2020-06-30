using Masny.Application.Interfaces;
using Masny.Domain.Models.Cloud;
using Microsoft.EntityFrameworkCore;

namespace Masny.Infrastructure.CloudContext
{
    /// <inheritdoc cref="ICloudDbContext"/>
    public class CloudDbContext : DbContext, ICloudDbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public CloudDbContext(DbContextOptions<CloudDbContext> options)
            : base(options) { }

        /// <inheritdoc/>
        public DbSet<User> Users { get; set; }

        /// <inheritdoc/>
        public DbSet<Post> Posts { get; set; }

        /// <inheritdoc/>
        public DbSet<Comment> Comments { get; set; }
    }
}
