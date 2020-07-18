using Masny.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Comment manager.
    /// </summary>
    public interface ICommentManager
    {
        /// <summary>
        /// Create new comment.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> CreateAsync(CommentDto commentDto);

        /// <summary>
        /// Get comment.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Comment data transfer object.</returns>
        Task<CommentDto> GetAsync(int id);

        /// <summary>
        /// Get comments.
        /// </summary>
        /// <returns>List of comments data transfer objects.</returns>
        Task<IEnumerable<CommentDto>> GetAllAsync();

        /// <summary>
        /// Update comment.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdateAsync(CommentDto commentDto);

        /// <summary>
        /// Delete comment.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Delete comment by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteByCloudIdAsync(int cloudId);
    }
}
