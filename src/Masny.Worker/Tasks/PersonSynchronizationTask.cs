using Coravel.Invocable;
using Masny.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Masny.Worker.Tasks
{
    /// <summary>
    /// Person synchronization task by Coravel.
    /// </summary>
    public class PersonSynchronizationTask : IInvocable
    {
        private readonly IPersonSynchronizationService _personSynchronizationService;

        public PersonSynchronizationTask(IPersonSynchronizationService personSynchronizationService)
        {
            _personSynchronizationService = personSynchronizationService ?? throw new ArgumentNullException(nameof(personSynchronizationService));
        }

        /// <inheritdoc/>
        public async Task Invoke()
        {
            await _personSynchronizationService.AddAsync();
            await _personSynchronizationService.DeleteAsync();
            await _personSynchronizationService.UpdateAsync();
        }
    }
}
