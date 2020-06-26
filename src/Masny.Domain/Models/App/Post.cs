using System.Collections.Generic;

namespace Masny.Domain.Models.App
{
    /// <summary>
    /// Post
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation to User.
        /// </summary>
        public Person User { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Collection for Comments.
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}
