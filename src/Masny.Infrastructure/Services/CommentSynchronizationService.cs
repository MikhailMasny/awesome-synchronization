using Masny.Application.Interfaces;
using Masny.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="ICommentSynchronizationService"/>
    public class CommentSynchronizationService : ICommentSynchronizationService
    {
        private readonly ICloudManager _cloudManager;
        private readonly ICommentManager _commentManager;
        private readonly IPostManager _postManager;

        public CommentSynchronizationService(ICloudManager cloudManager, ICommentManager commentManager, IPostManager postManager)
        {
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _commentManager = commentManager ?? throw new ArgumentNullException(nameof(commentManager));
            _postManager = postManager ?? throw new ArgumentNullException(nameof(postManager));
        }

        /// <inheritdoc/>
        public async Task Add()
        {
            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetCommentsWithoutTracking()).ToList();
            var postsApp = (await _postManager.GetPostsWithoutTracking()).ToList();

            var commentCloudIds = commentsCloud.Select(c => c.Id);
            var commentAppIds = commentsApp.Select(c => c.Id);

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

                        await _commentManager.CreateComment(commentDto);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public async Task Delete()
        {
            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetCommentsWithoutTracking()).ToList();

            var commentCloudIds = commentsCloud.Select(c => c.Id);
            var commentAppIds = commentsApp.Select(c => c.CloudId);

            var deleteIds = commentAppIds.Except(commentCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _commentManager.DeleteCommentByCloudId(id);
                }
            }
        }

        /// <inheritdoc/>
        public async Task Update()
        {
            var commentsCloud = await _cloudManager.GetComments().ToListAsync();
            var commentsApp = (await _commentManager.GetCommentsWithoutTracking()).ToList();

            foreach (var commentApp in commentsApp)
            {
                var commentCloud = commentsCloud.FirstOrDefault(c => c.Id == commentApp.CloudId);
                var postApp = await _postManager.GetPostWithoutTracking(commentApp.PostId);
                var isUpdated = false;

                if (postApp.CloudId != commentCloud.PostId)
                {
                    var postId = (await _postManager.GetPostWithoutTrackingByCloudId(commentCloud.Id)).Id;
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
                    await _commentManager.UpdateComment(commentApp);
                }
            }
        }
    }
}
