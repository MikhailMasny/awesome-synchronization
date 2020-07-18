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
        Task AddAsync();

        /// <summary>
        /// Delete posts.
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// Update posts.
        /// </summary>
        Task UpdateAsync();
    }
}
