using Coravel.Invocable;
using Masny.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Masny.Worker.Tasks
{
    /// <summary>
    /// Comment synchronization task by Coravel.
    /// </summary>
    public class CommentSynchronizationTask : IInvocable
    {
        private readonly ICommentSynchronizationService _commentSynchronizationService;

        public CommentSynchronizationTask(ICommentSynchronizationService commentSynchronizationService)
        {
            _commentSynchronizationService = commentSynchronizationService ?? throw new ArgumentNullException(nameof(commentSynchronizationService));
        }

        /// <inheritdoc/>
        public async Task Invoke()
        {
            await _commentSynchronizationService.Add();
            await _commentSynchronizationService.Delete();
            await _commentSynchronizationService.Update();
        }
    }
}
