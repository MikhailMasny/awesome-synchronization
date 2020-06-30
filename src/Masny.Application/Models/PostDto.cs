using Masny.Application.Interfaces;
using Masny.Domain.Models.App;

namespace Masny.Application.Models
{
    /// <summary>
    /// Post data transfer object.
    /// </summary>
    public class PostDto : IMapFrom<Post>
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
        /// User identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }
    }
}
