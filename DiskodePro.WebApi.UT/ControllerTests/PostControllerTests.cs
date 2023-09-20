using DiskodePro.WebApi.Controllers;
using DiskodePro.WebApi.Data.DTOs;
using DiskodePro.WebApi.Data.Repositories;
using DiskodePro.WebApi.Exceptions;
using DiskodePro.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DiskodePro.WebApi.UT.ControllerTests;

[TestFixture]
public class PostControllerTests
{
    private static bool IsSameActualAndExpectedPost(Post actual, Post expected)
    {
        return actual.CreatorId == expected.CreatorId
               && actual.PostId == expected.PostId
               && actual.Title == expected.Title
               && actual.Content == expected.Content;
    }
    
    [Test]
    public async Task GetPostByIdShouldReturnCorrectPost()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        var expectedPost = new Post
        {
            PostId = 1,
            Content = "",
            Title = "",
            CreatorId = 1
        };
        mockPostRepository.Setup(postRepository => postRepository.GetPostByIdAsync(expectedPost.PostId))
            .ReturnsAsync(expectedPost);
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var result = await postController.GetPostById(expectedPost.PostId);
        var okObject = result as OkObjectResult;
        var actualPost = okObject?.Value;
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(okObject, Is.Not.Null);
        Assert.That(actualPost, Is.Not.Null);
        Assert.That(IsSameActualAndExpectedPost((Post)actualPost!, expectedPost));
    }

    [Test]
    public async Task GetPostByIdShouldReturnNotFoundWhenPostNotFound()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        mockPostRepository.Setup(postRepository => postRepository.GetPostByIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new PostNotFoundException(""));
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var result = await postController.GetPostById(1);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task GetAllPostsShouldReturnAllPosts()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        var expectedPosts = new List<Post>
        {
            new Post
            {
                PostId = 1,
                CreatorId = 1,
                Title = "",
                Content = ""
            },
            new Post
            {
                PostId = 2,
                CreatorId = 2,
                Title = "",
                Content = ""
            }
        };
        mockPostRepository.Setup(postRepository => postRepository.GetAllPostsAsync())
            .ReturnsAsync(expectedPosts);
        var postController = new PostController(mockPostRepository.Object);

        // Act
        var result = await postController.GetAllPosts();
        var okObject = result as OkObjectResult;
        var actualPosts = okObject?.Value as List<Post>;
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(okObject, Is.Not.Null);
        Assert.That(actualPosts, Is.Not.Null);
        Assert.That(actualPosts!, Has.Count.EqualTo(expectedPosts.Count));
        for (var i = 0; i < actualPosts!.Count; i++)
            Assert.That(IsSameActualAndExpectedPost(actualPosts[i], expectedPosts[i]));
    }

    [Test]
    public async Task GetPostsByUserIdShouldReturnAllPostsByUser()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        var expectedPosts = new List<Post>
        {
            new Post
            {
                PostId = 1,
                CreatorId = 1,
                Title = "",
                Content = ""
            },
            new Post
            {
                PostId = 2,
                CreatorId = 1,
                Title = "",
                Content = ""
            }
        };
        mockPostRepository.Setup(postRepository => postRepository.GetPostsByUserIdAsync(expectedPosts[0].CreatorId))
            .ReturnsAsync(expectedPosts);
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var result = await postController.GetPostsByUserId(expectedPosts[0].CreatorId);
        var okObject = result as OkObjectResult;
        var actualPosts = okObject?.Value as List<Post>;
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(okObject, Is.Not.Null);
        Assert.That(actualPosts, Is.Not.Null);
        Assert.That(actualPosts!, Has.Count.EqualTo(expectedPosts.Count));
        for (var i = 0; i < actualPosts!.Count; i++)
            Assert.That(IsSameActualAndExpectedPost(actualPosts[i], expectedPosts[i]));
    }
    
    [Test]
    public async Task GetPostByUserIdShouldReturnNotFoundWhenUserNotFound()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        mockPostRepository.Setup(postRepository => postRepository.GetPostsByUserIdAsync(It.IsAny<int>()))
            .ThrowsAsync(new UserNotFoundException(""));
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var result = await postController.GetPostsByUserId(1);
        
        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreatePostShouldReturnCorrectCreatedPost()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        var inputPostDto = new PostDTO
        {
            CreatorId = 1,
            Title = "",
            Content = ""
        };
        var expectedPost = new Post
        {
            PostId = 1,
            CreatorId = inputPostDto.CreatorId,
            Title = inputPostDto.Title,
            Content = inputPostDto.Content
        };
        mockPostRepository.Setup(postRepository => postRepository.CreatePostAsync(inputPostDto)).ReturnsAsync(expectedPost);
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var actionResult = await postController.CreatePost(inputPostDto);
        var createdAction = actionResult as CreatedAtActionResult;
        var createdPost = createdAction?.Value as Post;

        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(createdAction, Is.Not.Null);
        Assert.That(createdPost, Is.Not.Null);
        Assert.That(IsSameActualAndExpectedPost(createdPost!, expectedPost));
    }

    [Test]
    public async Task CreatePostShouldReturnBadRequestWhenCreatorNotExists()
    {
        // Arrange
        var mockPostRepository = new Mock<IPostRepository>();
        mockPostRepository.Setup(postRepository => postRepository.CreatePostAsync(It.IsAny<PostDTO>()))
            .ThrowsAsync(new UserNotFoundException(""));
        var postController = new PostController(mockPostRepository.Object);
        
        // Act
        var actionResult = await postController.CreatePost(
            new PostDTO
            {
                CreatorId = 1,
                Title = "",
                Content = ""
            });
        
        // Assert
        Assert.That(actionResult, Is.Not.Null);
        Assert.That(actionResult, Is.InstanceOf<BadRequestObjectResult>());
    }
}