using AutoMapper;
using Masny.Application.Interfaces;
using Masny.Application.Managers;
using Masny.Application.Models;
using Masny.Domain.Models.App;
using Masny.Infrastructure.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Masny.UnitTests.Managers
{
    public class CommentManagerTests
    {
        private readonly ICommentManager _commentManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CommentManagerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            serviceCollection.AddAutoMapper(Assembly.Load("Masny.Application"));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _commentManager = new CommentManager(_appDbContext, _mapper);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommentManager(null, null));
            Assert.Throws<ArgumentNullException>(() => new CommentManager(_appDbContext, null));
        }

        [Fact]
        public void Method_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _commentManager.CreateAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentNullException>(() => _commentManager.UpdateAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreateComment_WhenEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var commentsCount = _appDbContext.Comments.Count();

            // Act
            var commentDto = new CommentDto
            {
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            var operationResult = _commentManager.CreateAsync(commentDto).GetAwaiter().GetResult();
            var commentsCountAfterOperation = _appDbContext.Comments.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(commentsCount, commentsCountAfterOperation);
        }

        [Fact]
        public void GetComment_WhenEmptyContext_ReturnsNull()
        {
            // Arrange
            var id = 1;

            // Act
            var commentDto = _commentManager.GetAsync(id).GetAwaiter().GetResult();

            // Assert
            Assert.Null(commentDto);
        }

        [Fact]
        public void GetComment_WhenNotEmptyContext_ReturnsComment()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();

            // Act
            var commentDto = _commentManager.GetAsync(comment.Id).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(commentDto);
            Assert.Equal(comment.Id, commentDto.Id);
        }

        [Fact]
        public void GetComments_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var commentDtos = _commentManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.Empty(commentDtos);
        }

        [Fact]
        public void GetComments_WhenNotEmptyContext_ReturnsComments()
        {
            // Arrange
            var commentOne = new Comment
            {
                Id = 1,
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            var commentTwo = new Comment
            {
                Id = 2,
                CloudId = 2,
                PostId = 2,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            _appDbContext.Comments.Add(commentOne);
            _appDbContext.Comments.Add(commentTwo);
            _appDbContext.SaveChanges();

            // Act
            var commentDtos = _commentManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.NotEmpty(commentDtos);
        }

        [Fact]
        public void UpdateComment_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange

            // Act
            var commentDto = new CommentDto
            {
                Id = 2,
                CloudId = 2,
                PostId = 2,
                Email = "anotherfake@fake.com",
                Name = "anotherfakename",
                Body = "anotherfakebody",
            };
            var operationResult = _commentManager.UpdateAsync(commentDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(0, operationResult);
        }

        [Fact]
        public void UpdateComment_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();

            // Act
            var commentDto = _commentManager.GetAsync(comment.Id).GetAwaiter().GetResult();
            commentDto.CloudId = 2;
            commentDto.PostId = 2;
            commentDto.Email = "anotherfake@fake.com";
            commentDto.Name = "anotherfakename";
            commentDto.Body = "anotherfakebody";

            var operationResult = _commentManager.UpdateAsync(commentDto).GetAwaiter().GetResult();
            var updatedComment = _appDbContext.Comments.FirstOrDefault(c => c.Id == comment.Id);

            // Assert
            Assert.Equal(1, operationResult);
            Assert.Equal(comment.CloudId, updatedComment.CloudId);
            Assert.Equal(comment.PostId, updatedComment.PostId);
            Assert.Equal(comment.Email, updatedComment.Email);
            Assert.Equal(comment.Name, updatedComment.Name);
            Assert.Equal(comment.Body, updatedComment.Body);
        }

        [Fact]
        public void DeleteComment_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var commentsCount = _appDbContext.Comments.Count();
            var id = 1;

            // Act
            var operationResult = _commentManager.DeleteAsync(id).GetAwaiter().GetResult();
            var commentsCountAfterOperation = _appDbContext.Comments.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(commentsCount, commentsCountAfterOperation);
        }

        [Fact]
        public void DeleteComment_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            var commentsCount = _appDbContext.Comments.Count();

            // Act
            var operationResult = _commentManager.DeleteAsync(comment.Id).GetAwaiter().GetResult();
            var commentsCountAfterOperation = _appDbContext.Comments.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(commentsCount, commentsCountAfterOperation);
        }

        [Fact]
        public void DeleteCommentByCloudId_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var commentsCount = _appDbContext.Comments.Count();
            var cloudId = 1;

            // Act
            var operationResult = _commentManager.DeleteByCloudIdAsync(cloudId).GetAwaiter().GetResult();
            var commentsCountAfterOperation = _appDbContext.Comments.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(commentsCount, commentsCountAfterOperation);
        }

        [Fact]
        public void DeleteCommentByCloudId_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                CloudId = 1,
                PostId = 1,
                Email = "fake@fake.com",
                Name = "fakename",
                Body = "fakebody",
            };
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            var commentsCount = _appDbContext.Comments.Count();

            // Act
            var operationResult = _commentManager.DeleteByCloudIdAsync(comment.CloudId).GetAwaiter().GetResult();
            var commentsCountAfterOperation = _appDbContext.Comments.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(commentsCount, commentsCountAfterOperation);
        }
    }
}
