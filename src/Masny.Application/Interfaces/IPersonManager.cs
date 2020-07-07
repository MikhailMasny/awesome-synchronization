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
        /// Get person without tracking.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Person data transfer object.</returns>
        Task<PersonDto> GetPersonWithoutTracking(int id);

        /// <summary>
        /// Get person without tracking by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Person data transfer object.</returns>
        Task<PersonDto> GetPersonWithoutTrackingByCloudId(int cloudId);

        /// <summary>
        /// Get people.
        /// </summary>
        /// <returns>List of person data transfer objects.</returns>
        Task<IEnumerable<PersonDto>> GetPeople();

        /// <summary>
        /// Get people without tracking.
        /// </summary>
        /// <returns>List of person data transfer objects.</returns>
        Task<IEnumerable<PersonDto>> GetPeopleWithoutTracking();

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

        /// <summary>
        /// Delete person by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeletePersonByCloudId(int cloudId);
    }
}
