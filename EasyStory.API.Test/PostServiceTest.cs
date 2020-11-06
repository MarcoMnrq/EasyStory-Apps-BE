using EasyStory.API.Domain.Models;
using EasyStory.API.Domain.Repositories;
using EasyStory.API.Domain.Services.Communications;
using EasyStory.API.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyStory.API.Test
{
    public class PostServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public async Task GetAllAsyncWhenNoPostsReturnsEmptyCollection()
        {
            // Arrange
            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            mockPostRepository.Setup(r => r.ListAsync())
                .ReturnsAsync(new List<Post>());
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var service = new PostService(
                mockPostRepository.Object,
                mockUnitOfWork.Object);

            // Act
            List<Post> posts = (List<Post>)await service.ListAsync();
            var postsCount = posts.Count;
            
            // Assert
            postsCount.Should().Equals(0);
        }

        [Test]
        public async Task GetByIdAsyncWhenInvalidIdReturnsPostNotFoundReResponse()
        {
            // Arrange
            var mockPostRepository = GetDefaultIPostRepositoryInstance();
            var postId = 1;
            mockPostRepository.Setup(r => r.FindById(postId))
                .Returns(Task.FromResult<Post>(null));
            var mockUnitOfWork = GetDefaultIUnitOfWorkInstance();
            var service = new PostService(
                mockPostRepository.Object,
                mockUnitOfWork.Object);


            // Act
            PostResponse response = await service.GetByIdAsync(postId);
            var message = response.Message;

            // Assert
            message.Should().Be("Post not found");
        }


        private Mock<IPostRepository> GetDefaultIPostRepositoryInstance()
        {
            return new Mock<IPostRepository>();
        }

        private Mock<IUnitOfWork> GetDefaultIUnitOfWorkInstance()
        {
            return new Mock<IUnitOfWork>();
        }
    }
}