using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Person synchronization service.
    /// </summary>
    public interface IPersonSynchronizationService
    {
        /// <summary>
        /// Add new people.
        /// </summary>
        Task AddAsync();

        /// <summary>
        /// Delete people.
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// Update people.
        /// </summary>
        Task UpdateAsync();
    }
}
