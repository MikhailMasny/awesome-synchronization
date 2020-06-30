using Masny.Application.Interfaces;
using Masny.Domain.Models.App;
using Masny.Infrastructure.AppContext.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Masny.Infrastructure.AppContext
{
    /// <inheritdoc cref="IAppDbContext"/>
    public class AppDbContext : DbContext, IAppDbContext
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        /// <inheritdoc/>
        public DbSet<Person> Persons { get; set; }

        /// <inheritdoc/>
        public DbSet<Post> Posts { get; set; }

        /// <inheritdoc/>
        public DbSet<Comment> Comments { get; set; }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
