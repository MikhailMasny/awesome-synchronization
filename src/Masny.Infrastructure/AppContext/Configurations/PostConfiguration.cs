using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masny.Infrastructure.AppContext.Configurations
{
    /// <summary>
    /// EF Configuration for post model.
    /// </summary>
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts")
                .HasKey(p => p.Id);
        }
    }
}
