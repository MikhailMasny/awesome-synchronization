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
    /// <inheritdoc cref="ICommentManager"/>
    public class CommentManager : ICommentManager
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public CommentManager(IAppDbContext context,
                              IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateAsync(CommentDto commentDto)
        {
            if (commentDto == null)
            {
                throw new ArgumentNullException(nameof(commentDto));
            }

            var comment = _mapper.Map<CommentDto, Comment>(commentDto);
            await _context.Comments.AddAsync(comment);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<CommentDto> GetAsync(int id)
        {
            var comment = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<Comment, CommentDto>(comment);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            var comments = await _context.Comments.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(CommentDto commentDto)
        {
            if (commentDto == null)
            {
                throw new ArgumentNullException(nameof(commentDto));
            }

            var comment = await _context.Comments.FindAsync(commentDto.Id);
            if (comment == null)
            {
                return 0;
            }

            // If have class context use:
            // entity = _mapper.Map<EntityDto, Entity>(entityDto);
            // _context.Entry(entity).State = EntityState.Modified;

            comment.CloudId = commentDto.CloudId;
            comment.PostId = commentDto.PostId;
            comment.Email = commentDto.Email;
            comment.Name = commentDto.Name;
            comment.Body = commentDto.Body;

            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return 0;
            }

            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteByCloudIdAsync(int cloudId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CloudId == cloudId);
            if (comment == null)
            {
                return 0;
            }

            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }
    }
}
