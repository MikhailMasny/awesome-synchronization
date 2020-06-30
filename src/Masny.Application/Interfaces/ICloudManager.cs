using Masny.Domain.Models.Cloud;
using System.Linq;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Clound manager.
    /// </summary>
    public interface ICloudManager
    {
        /// <summary>
        /// Get users.
        /// </summary>
        /// <returns>Database query.</returns>
        IQueryable<User> GetUsers();

        /// <summary>
        /// Get posts.
        /// </summary>
        /// <returns>Database query.</returns>
        IQueryable<Post> GetPosts();

        /// <summary>
        /// Get comments.
        /// </summary>
        /// <returns>Database query.</returns>
        IQueryable<Comment> GetComments();
    }
}
