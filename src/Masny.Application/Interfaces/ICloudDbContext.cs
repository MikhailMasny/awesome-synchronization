using Masny.Domain.Models.Cloud;
using Microsoft.EntityFrameworkCore;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Cloud database context.
    /// </summary>
    public interface ICloudDbContext
    {
        /// <summary>
        /// User entities.
        /// </summary>
        public DbSet<User> Users { get; set; }

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
