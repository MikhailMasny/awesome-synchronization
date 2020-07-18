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
    public class PostManagerTests
    {
        private readonly IPostManager _postManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public PostManagerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            serviceCollection.AddAutoMapper(Assembly.Load("Masny.Application"));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _postManager = new PostManager(_appDbContext, _mapper);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PostManager(null, null));
            Assert.Throws<ArgumentNullException>(() => new PostManager(_appDbContext, null));
        }

        [Fact]
        public void Method_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _postManager.CreateAsync(null).GetAwaiter().GetResult());
            Assert.Throws<ArgumentNullException>(() => _postManager.UpdateAsync(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void CreatePost_WhenEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var postsCount = _appDbContext.Posts.Count();

            // Act
            var postDto = new PostDto
            {
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            var operationResult = _postManager.CreateAsync(postDto).GetAwaiter().GetResult();
            var postsCountAfterOperation = _appDbContext.Posts.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(postsCount, postsCountAfterOperation);
        }

        [Fact]
        public void GetPost_WhenEmptyContext_ReturnsNull()
        {
            // Arrange
            var id = 1;

            // Act
            var postDto = _postManager.GetAsync(id).GetAwaiter().GetResult();

            // Assert
            Assert.Null(postDto);
        }

        [Fact]
        public void GetPost_WhenNotEmptyContext_ReturnsPost()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(post);
            _appDbContext.SaveChanges();

            // Act
            var postDto = _postManager.GetAsync(post.Id).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(postDto);
            Assert.Equal(post.Id, postDto.Id);
        }

        [Fact]
        public void GetPostByCloudId_WhenEmptyContext_ReturnsNull()
        {
            // Arrange
            var cloudId = 1;

            // Act
            var postDto = _postManager.GetByCloudIdAsync(cloudId).GetAwaiter().GetResult();

            // Assert
            Assert.Null(postDto);
        }

        [Fact]
        public void GetPostByCloudId_WhenNotEmptyContext_ReturnsPost()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(post);
            _appDbContext.SaveChanges();

            // Act
            var postDto = _postManager.GetByCloudIdAsync(post.CloudId).GetAwaiter().GetResult();

            // Assert
            Assert.NotNull(postDto);
            Assert.Equal(post.Id, postDto.Id);
        }

        [Fact]
        public void GetPosts_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var postsDtos = _postManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.Empty(postsDtos);
        }

        [Fact]
        public void GetPosts_WhenNotEmptyContext_ReturnsPosts()
        {
            // Arrange
            var postOne = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            var postTwo = new Post
            {
                Id = 2,
                CloudId = 2,
                UserId = 2,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(postOne);
            _appDbContext.Posts.Add(postTwo);
            _appDbContext.SaveChanges();

            // Act
            var postsDtos = _postManager.GetAllAsync().GetAwaiter().GetResult();

            // Assert
            Assert.NotEmpty(postsDtos);
        }

        [Fact]
        public void UpdatePost_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange

            // Act
            var postDto = new PostDto
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            var operationResult = _postManager.UpdateAsync(postDto).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(0, operationResult);
        }

        [Fact]
        public void UpdatePost_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(post);
            _appDbContext.SaveChanges();

            // Act
            var postDto = _postManager.GetAsync(post.Id).GetAwaiter().GetResult();
            postDto.CloudId = 2;
            postDto.UserId = 2;
            postDto.Title = "anotherfaketitle";
            postDto.Body = "anotherfakebody";

            var operationResult = _postManager.UpdateAsync(postDto).GetAwaiter().GetResult();
            var updatedPost = _appDbContext.Posts.FirstOrDefault(c => c.Id == post.Id);

            // Assert
            Assert.Equal(1, operationResult);
            Assert.Equal(post.CloudId, updatedPost.CloudId);
            Assert.Equal(post.UserId, updatedPost.UserId);
            Assert.Equal(post.Title, updatedPost.Title);
            Assert.Equal(post.Body, updatedPost.Body);
        }

        [Fact]
        public void DeletePost_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var postsCount = _appDbContext.Posts.Count();
            var id = 1;

            // Act
            var operationResult = _postManager.DeleteAsync(id).GetAwaiter().GetResult();
            var postsCountAfterOperation = _appDbContext.Posts.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(postsCount, postsCountAfterOperation);
        }

        [Fact]
        public void DeletePost_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(post);
            _appDbContext.SaveChanges();
            var postsCount = _appDbContext.Posts.Count();

            // Act
            var operationResult = _postManager.DeleteAsync(post.Id).GetAwaiter().GetResult();
            var postsCountAfterOperation = _appDbContext.Posts.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(postsCount, postsCountAfterOperation);
        }

        [Fact]
        public void DeletePostByCloudId_WhenEmptyContext_ReturnsUnsuccessfulOperation()
        {
            // Arrange
            var postsCount = _appDbContext.Posts.Count();
            var cloudId = 1;

            // Act
            var operationResult = _postManager.DeleteByCloudIdAsync(cloudId).GetAwaiter().GetResult();
            var postsCountAfterOperation = _appDbContext.Posts.Count();

            // Assert
            Assert.Equal(0, operationResult);
            Assert.Equal(postsCount, postsCountAfterOperation);
        }

        [Fact]
        public void DeletePostByCloudId_WhenNotEmptyContext_ReturnsSuccessfulOperation()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                CloudId = 1,
                UserId = 1,
                Title = "faketitle",
                Body = "fakebody",
            };
            _appDbContext.Posts.Add(post);
            _appDbContext.SaveChanges();
            var postsCount = _appDbContext.Posts.Count();

            // Act
            var operationResult = _postManager.DeleteByCloudIdAsync(post.CloudId).GetAwaiter().GetResult();
            var postsCountAfterOperation = _appDbContext.Posts.Count();

            // Assert
            Assert.Equal(1, operationResult);
            Assert.NotEqual(postsCount, postsCountAfterOperation);
        }
    }
}
