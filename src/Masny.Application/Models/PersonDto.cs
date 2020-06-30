using Masny.Application.Interfaces;
using Masny.Domain.Models.App;

namespace Masny.Application.Models
{
    /// <summary>
    /// Person data transfer object.
    /// </summary>
    public class PersonDto : IMapFrom<Person>
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
    }
}
