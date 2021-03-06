﻿using Masny.Application.Interfaces;
using Masny.Domain.Models.Cloud;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Masny.Application.Managers
{
    /// <inheritdoc cref="ICloudManager"/>
    public class CloudManager : ICloudManager
    {
        private readonly ICloudDbContext _context;

        public CloudManager(ICloudDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public IQueryable<User> GetUsers()
        {
            return _context.Users.AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<Post> GetPosts()
        {
            return _context.Posts.AsNoTracking();
        }

        /// <inheritdoc/>
        public IQueryable<Comment> GetComments()
        {
            return _context.Comments.AsNoTracking();
        }
    }
}
