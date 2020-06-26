namespace Masny.Domain.Models.Cloud
{
    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Website.
        /// </summary>
        public string Website { get; set; }
    }
}
