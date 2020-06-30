using Coravel.Invocable;
using Masny.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Masny.Worker.Tasks
{
    public class PeopleSyncTask : IInvocable
    {
        private readonly IPersonSynchronizationService _personSynchronizationService;

        public PeopleSyncTask(IPersonSynchronizationService personSynchronizationService)
        {
            _personSynchronizationService = personSynchronizationService ?? throw new ArgumentNullException(nameof(personSynchronizationService));
        }

        public async Task Invoke()
        {
            await _personSynchronizationService.AddNewPeople();
        }
    }
}
