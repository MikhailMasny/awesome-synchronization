using Masny.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
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
        Task<int> CreatePerson(PersonDto personDto);

        /// <summary>
        /// Get person.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Person data transfer object.</returns>
        Task<PersonDto> GetPerson(int id);

        /// <summary>
        /// Get people.
        /// </summary>
        /// <returns>List of person data transfer objects.</returns>
        Task<IEnumerable<PersonDto>> GetPeople();

        /// <summary>
        /// Update person.
        /// </summary>
        /// <param name="personDto">Person data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdatePerson(PersonDto personDto);

        /// <summary>
        /// Delete person.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeletePerson(int id);
    }
}
