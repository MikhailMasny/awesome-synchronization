using System.Collections.Generic;

namespace Masny.Domain.Models.App
{
    /// <summary>
    /// Person.
    /// </summary>
    public class Person
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
        /// Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Collection for Posts.
        /// </summary>
        public ICollection<Post> Posts { get; set; }
    }
}
