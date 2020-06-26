using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masny.Infrastructure.AppContext.Configurations
{
    /// <summary>
    /// EF Configuration for user model.
    /// </summary>
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Persons")
                .HasKey(p => p.Id);
        }
    }
}
