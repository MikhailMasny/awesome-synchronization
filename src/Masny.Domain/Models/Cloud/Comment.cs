﻿namespace Masny.Domain.Models.Cloud
{
    /// <summary>
    /// Comment.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

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
