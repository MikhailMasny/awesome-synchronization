using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="ICommentSynchronizationService"/>
    public class CommentSynchronizationService : ICommentSynchronizationService
    {
        private readonly ILogger<CommentSynchronizationService> _logger;
        private readonly ICloudManager _cloudManager;
        private readonly ICommentManager _commentManager;
        private readonly IPostManager _postManager;

        public CommentSynchronizationService(ILogger<CommentSynchronizationService> logger, ICloudManager cloudManager, ICommentManager commentManager, IPostManager postManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _commentManager = commentManager ?? throw new ArgumentNullException(nameof(commentManager));
            _postManager = postManager ?? throw new ArgumentNullException(nameof(postManager));
        }

        /// <inheritdoc/>
        public async Task AddAsync()
        {
            _logger.LogInformation(Messages.CommentAddStart);

            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetAllAsync()).ToList();
            var postsApp = (await _postManager.GetAllAsync()).ToList();

            var commentCloudIds = commentsCloud.Select(c => c.Id);
            var commentAppIds = commentsApp.Select(c => c.CloudId); // TODO: fix everywhere

            var newIds = commentCloudIds.Except(commentAppIds);
            var comments = commentsCloud.Join(
                newIds,
                commentCloud => commentCloud.Id,
                id => id,
                (commentCloud, id) => commentCloud);

            if (comments.Any())
            {
                foreach (var comment in comments)
                {
                    var post = postsApp.FirstOrDefault(p => p.CloudId == comment.PostId);

                    if (post != null)
                    {
                        var commentDto = new CommentDto
                        {
                            CloudId = comment.Id,
                            PostId = post.Id,
                            Name = comment.Name,
                            Email = comment.Email,
                            Body = comment.Body
                        };

                        await _commentManager.CreateAsync(commentDto);
                    }
                    else
                    {
                        _logger.LogError(Messages.CommentAddError, comment.PostId);
                    }
                }
            }

            _logger.LogInformation(Messages.CommentAddEnd);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync()
        {
            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetAllAsync()).ToList();

            var commentCloudIds = commentsCloud.Select(c => c.Id);
            var commentAppIds = commentsApp.Select(c => c.CloudId);

            var deleteIds = commentAppIds.Except(commentCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _commentManager.DeleteByCloudIdAsync(id);
                }
            }
        }

        /// <inheritdoc/>
        public async Task UpdateAsync()
        {
            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetAllAsync()).ToList();

            foreach (var commentApp in commentsApp)
            {
                var commentCloud = commentsCloud.FirstOrDefault(c => c.Id == commentApp.CloudId);
                var postApp = await _postManager.GetAsync(commentApp.PostId);
                var isUpdated = false;

                if (postApp.CloudId != commentCloud.PostId)
                {
                    var postId = (await _postManager.GetByCloudIdAsync(commentCloud.Id)).Id;
                    commentApp.PostId = postId;
                    isUpdated = true;
                }

                if (commentApp.Name != commentCloud.Name)
                {
                    commentApp.Name = commentCloud.Name;
                    isUpdated = true;
                }

                if (commentApp.Email != commentCloud.Email)
                {
                    commentApp.Email = commentCloud.Email;
                    isUpdated = true;
                }

                if (commentApp.Body != commentCloud.Body)
                {
                    commentApp.Body = commentCloud.Body;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await _commentManager.UpdateAsync(commentApp);
                }
            }
        }
    }
}
