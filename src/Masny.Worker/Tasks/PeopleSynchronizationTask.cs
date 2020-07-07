using Coravel.Invocable;
using Masny.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Masny.Worker.Tasks
{
    /// <summary>
    /// People synchronization task by Coravel.
    /// </summary>
    public class PeopleSynchronizationTask : IInvocable
    {
        private readonly IPersonSynchronizationService _personSynchronizationService;

        public PeopleSynchronizationTask(IPersonSynchronizationService personSynchronizationService)
        {
            _personSynchronizationService = personSynchronizationService ?? throw new ArgumentNullException(nameof(personSynchronizationService));
        }

        /// <inheritdoc/>
        public async Task Invoke()
        {
            await _personSynchronizationService.AddPeople();
            await _personSynchronizationService.DeletePeople();
            await _personSynchronizationService.UpdatePeople();
        }
    }
}
