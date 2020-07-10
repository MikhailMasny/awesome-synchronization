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
        Task<int> CreateComment(CommentDto commentDto);

        /// <summary>
        /// Get comment.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Comment data transfer object.</returns>
        Task<CommentDto> GetComment(int id);

        /// <summary>
        /// Get comments.
        /// </summary>
        /// <returns>List of comment data transfer objects.</returns>
        Task<IEnumerable<CommentDto>> GetComments();

        /// <summary>
        /// Get comments without tracking.
        /// </summary>
        /// <returns>List of comments data transfer objects.</returns>
        Task<IEnumerable<CommentDto>> GetCommentsWithoutTracking();

        /// <summary>
        /// Update comment.
        /// </summary>
        /// <param name="commentDto">Comment data transfer object.</param>
        /// <returns>Operation result.</returns>
        Task<int> UpdateComment(CommentDto commentDto);

        /// <summary>
        /// Delete comment.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteComment(int id);

        /// <summary>
        /// Delete comment by cloud identifier.
        /// </summary>
        /// <param name="cloudId">Cloud identifier.</param>
        /// <returns>Operation result.</returns>
        Task<int> DeleteCommentByCloudId(int cloudId);
    }
}
