using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Comment synchronization service.
    /// </summary>
    public interface ICommentSynchronizationService
    {
        /// <summary>
        /// Add new comments.
        /// </summary>
        Task AddAsync();

        /// <summary>
        /// Delete comments.
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// Update comments.
        /// </summary>
        Task UpdateAsync();
    }
}
