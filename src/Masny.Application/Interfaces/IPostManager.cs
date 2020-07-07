using Masny.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Post manager.
    /// </summary>
    public interface IPostManager
    {
        /// <summary>
        /// Create new post.
        /// </summary>
        /// <param name="postDto">Post data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> CreatePost(PostDto postDto);

        /// <summary>
        /// Get post.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Post data transfer object.</returns>
        Task<PostDto> GetPost(int id);

        /// <summary>
        /// Get posts.
        /// </summary>
        /// <returns>List of post data transfer objects.</returns>
        Task<IEnumerable<PostDto>> GetPosts();

        /// <summary>
        /// Get posts without tracking.
        /// </summary>
        /// <returns>List of posts data transfer objects.</returns>
        Task<IEnumerable<PostDto>> GetPostsWithoutTracking();

        /// <summary>
        /// Update post.
        /// </summary>
        /// <param name="postDto">Post data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdatePost(PostDto postDto);

        /// <summary>
        /// Delete post.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeletePost(int id);

        /// <summary>
        /// Delete post by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeletePostByCloudId(int cloudId);
    }
}
