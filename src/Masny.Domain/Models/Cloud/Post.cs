namespace Masny.Domain.Models.Cloud
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
        /// Title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Body.
        /// </summary>
        public string Body { get; set; }
    }
}
