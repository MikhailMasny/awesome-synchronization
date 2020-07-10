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

        public PostManager(IAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreatePost(PostDto postDto)
        {
            // TODO: use Serilog
            var post = _mapper.Map<PostDto, Post>(postDto);
            await _context.Posts.AddAsync(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<PostDto> GetPost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Post, PostDto>(post);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PostDto>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PostDto>> GetPostsWithoutTracking()
        {
            var posts = await _context.Posts.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
        }

        /// <inheritdoc/>
        public async Task<int> UpdatePost(PostDto postDto)
        {
            // TODO: Serilog
            var post = _mapper.Map<PostDto, Post>(postDto);
            _context.Posts.Update(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeletePost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            _context.Posts.Remove(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeletePostByCloudId(int cloudId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.CloudId == cloudId);
            _context.Posts.Remove(post);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<PostDto> GetPostWithoutTracking(int id)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return _mapper.Map<Post, PostDto>(post);
        }

        /// <inheritdoc/>
        public async Task<PostDto> GetPostWithoutTrackingByCloudId(int cloudId)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.CloudId == cloudId);
            return _mapper.Map<Post, PostDto>(post);
        }
    }
}
