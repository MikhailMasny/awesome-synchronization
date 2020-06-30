using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Masny.Application.Interfaces
{
    /// <summary>
    /// Application database context.
    /// </summary>
    public interface IAppDbContext
    {
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

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns>Operation result.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
