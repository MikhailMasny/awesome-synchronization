using Masny.Application.Interfaces;
using Masny.Domain.Models.App;

namespace Masny.Application.Models
{
    /// <summary>
    /// Comment data transfer object
    /// </summary>
    public class CommentDto : IMapFrom<Comment>
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Cloud identifier.
        /// </summary>
        public int CloudId { get; set; }

        /// <summary>
        /// Post identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }
    }
}
