using Masny.Application.Interfaces;
using Masny.Application.Managers;
using Masny.Domain.Models.Cloud;
using Masny.Infrastructure.CloudContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Masny.UnitTests.Managers
{
    public class CloudManagerTests
    {
        private readonly ICloudManager _cloudManager;
        private readonly CloudDbContext _cloudDbContext;

        public CloudManagerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<CloudDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _cloudDbContext = serviceProvider.GetRequiredService<CloudDbContext>();

            _cloudManager = new CloudManager(_cloudDbContext);
        }

        [Fact]
        public void Constructor_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CloudManager(null));
        }

        [Fact]
        public void GetUsers_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange
            var usersCount = _cloudDbContext.Users.Count();

            // Act
            var users = _cloudManager.GetUsers().ToList();

            // Assert
            Assert.Empty(users);
            Assert.Equal(users.Count, usersCount);
        }

        [Fact]
        public void GetUsers_WhenNotEmptyContext_ReturnsUsers()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "fakename",
                Email = "email@fake.fake",
                Name = "fake",
                Phone = "1234567",
                Website = "fake.com"
            };

            _cloudDbContext.Users.Add(user);
            _cloudDbContext.SaveChanges();

            var usersCount = _cloudDbContext.Users.Count();

            // Act
            var users = _cloudManager.GetUsers().ToList();

            // Assert
            Assert.NotEmpty(users);
            Assert.Equal(users.Count, usersCount);
        }

        [Fact]
        public void GetPosts_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange
            var postsCount = _cloudDbContext.Posts.Count();

            // Act
            var posts = _cloudManager.GetPosts().ToList();

            // Assert
            Assert.Empty(posts);
            Assert.Equal(posts.Count, postsCount);
        }

        [Fact]
        public void GetPosts_WhenNotEmptyContext_ReturnsPosts()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                UserId = 1,
                Body = "fakebody",
                Title = "faketitle"
            };

            _cloudDbContext.Posts.Add(post);
            _cloudDbContext.SaveChanges();

            var postsCount = _cloudDbContext.Posts.Count();

            // Act
            var posts = _cloudManager.GetPosts().ToList();

            // Assert
            Assert.NotEmpty(posts);
            Assert.Equal(posts.Count, postsCount);
        }

        [Fact]
        public void GetComments_WhenEmptyContext_ReturnsEmptyList()
        {
            // Arrange
            var commentsCount = _cloudDbContext.Comments.Count();

            // Act
            var comments = _cloudManager.GetComments().ToList();

            // Assert
            Assert.Empty(comments);
            Assert.Equal(comments.Count, commentsCount);
        }

        [Fact]
        public void GetComments_WhenNotEmptyContext_ReturnsComments()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                PostId = 1,
                Body = "fakebody",
                Email = "email@fake.fake",
                Name = "fake"
            };

            _cloudDbContext.Comments.Add(comment);
            _cloudDbContext.SaveChanges();

            var commentsCount = _cloudDbContext.Comments.Count();

            // Act
            var comments = _cloudManager.GetComments().ToList();

            // Assert
            Assert.NotEmpty(comments);
            Assert.Equal(comments.Count, commentsCount);
        }
    }
}
