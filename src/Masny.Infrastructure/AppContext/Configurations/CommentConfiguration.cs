using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Masny.Infrastructure.AppContext.Configurations
{
    /// <summary>
    /// EF Configuration for comment model.
    /// </summary>
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments")
                .HasKey(c => c.Id);
        }
    }
}
