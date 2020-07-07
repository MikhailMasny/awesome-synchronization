using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Post synchronization service.
    /// </summary>
    public interface IPostSynchronizationService
    {
        /// <summary>
        /// Add new posts.
        /// </summary>
        Task AddPosts();

        /// <summary>
        /// Delete posts.
        /// </summary>
        Task DeletePosts();

        /// <summary>
        /// Update posts.
        /// </summary>
        Task UpdatePosts();
    }
}
