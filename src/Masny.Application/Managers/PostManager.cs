using AutoMapper;
using Masny.Application.Interfaces;
using Masny.Application.Models;
using Masny.Domain.Models.App;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masny.Application.Managers
{
    /// <inheritdoc cref="IPostManager"/>
    public class PostManager : IPostManager
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public PostManager(IAppDbContext context,
                           IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(PostDto postDto)
        {
            if (postDto == null)
            {
                throw new ArgumentNullException(nameof(postDto));
            }

            var post = _mapper.Map<PostDto, Post>(postDto);
            await _context.Posts.AddAsync(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<PostDto> GetAsync(int id)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Post, PostDto>(post);
        }

        /// <inheritdoc/>
        public async Task<PostDto> GetByCloudIdAsync(int cloudId)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.CloudId == cloudId);
            return _mapper.Map<Post, PostDto>(post);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PostDto>> GetAllAsync()
        {
            var posts = await _context.Posts.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(PostDto postDto)
        {
            if (postDto == null)
            {
                throw new ArgumentNullException(nameof(postDto));
            }

            var post = await _context.Posts.FindAsync(postDto.Id);
            if (post == null)
            {
                return 0;
            }

            // See comment at CommentManager file

            post.CloudId = postDto.CloudId;
            post.UserId = postDto.UserId;
            post.Title = postDto.Title;
            post.Body = postDto.Body;

            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return 0;
            }

            _context.Posts.Remove(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteByCloudIdAsync(int cloudId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.CloudId == cloudId);
            if (post == null)
            {
                return 0;
            }

            _context.Posts.Remove(post);
            return await _context.SaveChangesAsync();
        }
    }
}
