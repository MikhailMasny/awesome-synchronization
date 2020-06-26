using Masny.Domain.Models.App;
using Masny.Infrastructure.AppContext.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Masny.Infrastructure.AppContext
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        /// <summary>
        /// Person entities.
        /// </summary>
        public DbSet<Person> Persons { get; set; }

        /// <summary>
        /// Post entities.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Comment entities.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
