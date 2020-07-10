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

        public CommentManager(IAppDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<int> CreateComment(CommentDto commentDto)
        {
            var comment = _mapper.Map<CommentDto, Comment>(commentDto);
            await _context.Comments.AddAsync(comment);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteComment(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<int> DeleteCommentByCloudId(int cloudId)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CloudId == cloudId);
            _context.Comments.Remove(comment);
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<CommentDto> GetComment(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<Comment, CommentDto>(comment);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommentDto>> GetComments()
        {
            var comments = await _context.Comments.ToListAsync();
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommentDto>> GetCommentsWithoutTracking()
        {
            var comments = await _context.Comments.AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateComment(CommentDto commentDto)
        {
            // TODO: Serilog
            var comment = _mapper.Map<CommentDto, Comment>(commentDto);
            _context.Comments.Update(comment);
            return await _context.SaveChangesAsync();
        }
    }
}
