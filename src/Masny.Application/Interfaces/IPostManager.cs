using Masny.Application.Models;
using System.Collections.Generic;
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
        Task<int> CreateAsync(PostDto postDto);

        /// <summary>
        /// Get post without tracking.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Post data transfer object.</returns>
        Task<PostDto> GetAsync(int id);

        /// <summary>
        /// Get post without tracking by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Post data transfer object.</returns>
        Task<PostDto> GetByCloudIdAsync(int cloudId);

        /// <summary>
        /// Get posts without tracking.
        /// </summary>
        /// <returns>List of posts data transfer objects.</returns>
        Task<IEnumerable<PostDto>> GetAllAsync();

        /// <summary>
        /// Update post.
        /// </summary>
        /// <param name="postDto">Post data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdateAsync(PostDto postDto);

        /// <summary>
        /// Delete post.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Delete post by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteByCloudIdAsync(int cloudId);
    }
}
