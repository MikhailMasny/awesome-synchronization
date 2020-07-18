using Coravel.Invocable;
using Masny.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Masny.Worker.Tasks
{
    /// <summary>
    /// Person synchronization task by Coravel.
    /// </summary>
    public class PostSynchronizationTask : IInvocable
    {
        private readonly IPostSynchronizationService _postSynchronizationService;

        public PostSynchronizationTask(IPostSynchronizationService postSynchronizationService)
        {
            _postSynchronizationService = postSynchronizationService ?? throw new ArgumentNullException(nameof(postSynchronizationService));
        }

        /// <inheritdoc/>
        public async Task Invoke()
        {
            await _postSynchronizationService.AddAsync();
            await _postSynchronizationService.DeleteAsync();
            await _postSynchronizationService.UpdateAsync();
        }
    }
}
