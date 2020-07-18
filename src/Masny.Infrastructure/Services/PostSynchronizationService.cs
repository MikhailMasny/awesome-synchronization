using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Application.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="IPostSynchronizationService"/>
    public class PostSynchronizationService : IPostSynchronizationService
    {
        private readonly ILogger<PostSynchronizationService> _logger;
        private readonly ICloudManager _cloudManager;
        private readonly IPostManager _postManager;
        private readonly IPersonManager _personManager;

        public PostSynchronizationService(ILogger<PostSynchronizationService> logger,
                                          ICloudManager cloudManager,
                                          IPostManager postManager,
                                          IPersonManager personManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _postManager = postManager ?? throw new ArgumentNullException(nameof(postManager));
            _personManager = personManager ?? throw new ArgumentNullException(nameof(personManager));
        }

        /// <inheritdoc/>
        public async Task AddAsync()
        {
            _logger.LogInformation(SyncMessage.PostAddStart);

            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetAllAsync()).ToList();
            var peopleApp = (await _personManager.GetAllAsync()).ToList();

            var postsCloudIds = postsCloud.Select(p => p.Id);
            var postsAppIds = postsApp.Select(p => p.CloudId);

            var newIds = postsCloudIds.Except(postsAppIds);

            var posts = postsCloud.Join(
                newIds,
                postCloud => postCloud.Id,
                id => id,
                (postCloud, id) => postCloud);

            if (posts.Any())
            {
                foreach (var post in posts)
                {
                    var user = peopleApp.FirstOrDefault(p => p.CloudId == post.UserId);

                    if (user != null)
                    {
                        var postDto = new PostDto
                        {
                            CloudId = post.Id,
                            UserId = user.Id,
                            Title = post.Title,
                            Body = post.Body
                        };

                        await _postManager.CreateAsync(postDto);
                    }
                    else
                    {
                        _logger.LogError(SyncMessage.PostAddError, post.UserId);
                    }
                }
            }

            _logger.LogInformation(SyncMessage.PostAddEnd);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync()
        {
            _logger.LogInformation(SyncMessage.PostDeleteStart);

            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetAllAsync()).ToList();

            var postsCloudIds = postsCloud.Select(p => p.Id);
            var postsAppIds = postsApp.Select(p => p.CloudId);

            var deleteIds = postsAppIds.Except(postsCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _postManager.DeleteByCloudIdAsync(id);
                }
            }

            _logger.LogInformation(SyncMessage.PostDeleteEnd);
        }

        /// <inheritdoc/>
        public async Task UpdateAsync()
        {
            _logger.LogInformation(SyncMessage.PostUpdateStart);

            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetAllAsync()).ToList();

            foreach (var postApp in postsApp)
            {
                var postCloud = postsCloud.FirstOrDefault(p => p.Id == postApp.CloudId);
                var personApp = await _personManager.GetAsync(postApp.UserId);
                var isUpdated = false;

                if (personApp.CloudId != postCloud.UserId)
                {
                    var personId = (await _personManager.GetByCloudIdAsync(postCloud.UserId)).Id;
                    postApp.UserId = personId;
                    isUpdated = true;
                }

                if (postApp.Title != postCloud.Title)
                {
                    postApp.Title = postCloud.Title;
                    isUpdated = true;
                }

                if (postApp.Body != postCloud.Body)
                {
                    postApp.Body = postCloud.Body;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await _postManager.UpdateAsync(postApp);
                }
            }

            _logger.LogInformation(SyncMessage.PostUpdateEnd);
        }
    }
}
