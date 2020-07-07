﻿using System.Threading.Tasks;

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
        Task AddPeople();

        /// <summary>
        /// Delete people.
        /// </summary>
        Task DeletePeople();

        /// <summary>
        /// Update people.
        /// </summary>
        Task UpdatePeople();
    }
}
