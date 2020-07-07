using Masny.Application.Interfaces;
using Masny.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masny.Infrastructure.Services
{
    /// <inheritdoc cref="IPostSynchronizationService"/>
    public class PostSynchronizationService : IPostSynchronizationService
    {
        private readonly ICloudManager _cloudManager;
        private readonly IPostManager _postManager;
        private readonly IPersonManager _personManager;

        public PostSynchronizationService(ICloudManager cloudManager, IPostManager postManager, IPersonManager personManager)
        {
            _cloudManager = cloudManager ?? throw new ArgumentNullException(nameof(cloudManager));
            _postManager = postManager ?? throw new ArgumentNullException(nameof(postManager));
            _personManager = personManager ?? throw new ArgumentNullException(nameof(personManager));
        }

        /// <inheritdoc/>
        public async Task AddPosts()
        {
            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetPostsWithoutTracking()).ToList();
            var peopleApp = (await _personManager.GetPeopleWithoutTracking()).ToList();

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
                    var userId = peopleApp.FirstOrDefault(p => p.CloudId == post.UserId).Id;

                    var postDto = new PostDto
                    {
                        CloudId = post.Id,
                        UserId = userId,
                        Title = post.Title,
                        Body = post.Body
                    };

                    await _postManager.CreatePost(postDto);
                }
            }
        }

        /// <inheritdoc/>
        public async Task DeletePosts()
        {
            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetPostsWithoutTracking()).ToList();

            var postsCloudIds = postsCloud.Select(p => p.Id);
            var postsAppIds = postsApp.Select(p => p.CloudId);

            var deleteIds = postsAppIds.Except(postsCloudIds);

            if (deleteIds.Any())
            {
                foreach (var id in deleteIds)
                {
                    await _postManager.DeletePostByCloudId(id);
                }
            }
        }

        /// <inheritdoc/>
        public async Task UpdatePosts()
        {
            var postsCloud = await _cloudManager.GetPosts().ToListAsync();
            var postsApp = (await _postManager.GetPostsWithoutTracking()).ToList();

            foreach (var postApp in postsApp)
            {
                var postCloud = postsCloud.FirstOrDefault(p => p.Id == postApp.CloudId);
                var personApp = await _personManager.GetPersonWithoutTracking(postApp.UserId);
                var isUpdated = false;

                if (personApp.CloudId != postCloud.UserId)
                {
                    var personId = (await _personManager.GetPersonWithoutTrackingByCloudId(postCloud.UserId)).Id;
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
                    await _postManager.UpdatePost(postApp);
                }
            }
        }
    }
}
