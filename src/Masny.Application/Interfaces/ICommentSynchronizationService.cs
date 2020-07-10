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
        Task Add();

        /// <summary>
        /// Delete comments.
        /// </summary>
        Task Delete();

        /// <summary>
        /// Update comments.
        /// </summary>
        Task Update();
    }
}
