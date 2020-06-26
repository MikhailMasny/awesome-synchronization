using Masny.Domain.Models.Cloud;
using Microsoft.EntityFrameworkCore;

namespace Masny.Infrastructure.CloudContext
{
    /// <summary>
    /// Cloud database context.
    /// </summary>
    public class CloudDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public CloudDbContext(DbContextOptions<CloudDbContext> options)
            : base(options) { }

        /// <summary>
        /// User entities.
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Post entities.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Comment entities.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }
    }
}
