using Masny.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Person manager.
    /// </summary>
    public interface IPersonManager
    {
        /// <summary>
        /// Create new person.
        /// </summary>
        /// <param name="personDto">Person data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> CreateAsync(PersonDto personDto);

        /// <summary>
        /// Get person without tracking.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Person data transfer object.</returns>
        Task<PersonDto> GetAsync(int id);

        /// <summary>
        /// Get person without tracking by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Person data transfer object.</returns>
        Task<PersonDto> GetByCloudIdAsync(int cloudId);

        /// <summary>
        /// Get people without tracking.
        /// </summary>
        /// <returns>List of person data transfer objects.</returns>
        Task<IEnumerable<PersonDto>> GetAllAsync();

        /// <summary>
        /// Update person.
        /// </summary>
        /// <param name="personDto">Person data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdateAsync(PersonDto personDto);

        /// <summary>
        /// Delete person.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Delete person by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteByCloudIdAsync(int cloudId);
    }
}
