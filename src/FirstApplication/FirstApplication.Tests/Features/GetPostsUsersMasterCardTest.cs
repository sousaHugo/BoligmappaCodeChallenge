using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetPostsUsersMasterCard;
using FirstApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features;

public  class GetPostsUsersMasterCardTest
{
    private readonly GetPostsUsersMasterCardRequestHandler _handler;

    private readonly Mock<IPostService> _postServiceMock;
    private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
    private readonly Mock<ILogger<GetPostsUsersMasterCardRequestHandler>> _loggerMock;
    
    private readonly List<PostDto> _dtoPosts = new();
    private readonly List<UserInfo> _dbUsers = new();

    public GetPostsUsersMasterCardTest()
    {
        _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
        _postServiceMock = new Mock<IPostService>();
        _loggerMock = new Mock<ILogger<GetPostsUsersMasterCardRequestHandler>>();

        _userInfoRepositoryMock.Setup(m => m.GetUserUseMasterCardAsync()).ReturnsAsync(() => {
            return _dbUsers.Where(a => a.UseMasterCard);
        
        });
        _postServiceMock.Setup(m => m.GetAllPostsByUsernameAsync<IEnumerable<PostDto>>(It.IsAny<string>())).ReturnsAsync((string Username) => {

            return _dtoPosts.Where(a => a.Username == Username);
        });

        _handler = new GetPostsUsersMasterCardRequestHandler(_postServiceMock.Object, _userInfoRepositoryMock.Object, _loggerMock.Object);
    }
    [Fact]
    public void GetPostsUsersMasterCardRequestHandler_NoResultsSuccess()
    {
        var result = _handler.Handle(new GetPostsUsersMasterCardRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
        result.ShouldBeEmpty();
    }
    [Fact]
    public void GetPostsUsersMasterCardRequestHandler_OneResultSuccess()
    {
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "john.doe",
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = true,
            UserId = "1234"
        });
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "rose.doe",
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = false,
            UserId = "12345"
        });

        _dtoPosts.Add(new PostDto()
        {
            Username = "john.doe",
            Id = "P1",
            Post = "Post"
        });
        _dtoPosts.Add(new PostDto()
        {
            Username = "rose.doe",
            Id = "P1",
            Post = "Post"
        });

        var result = _handler.Handle(new GetPostsUsersMasterCardRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
        result.ShouldHaveSingleItem();
    }

    [Fact]
    public void GetPostsUsersMasterCardRequestHandler_OneResultEqualInfoSuccess()
    {
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "john.doe",
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = true,
            UserId = "1234"
        });
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "rose.doe",
            NumberOfPosts = 1,
            NumberOfTodos = 1,
            UseMasterCard = false,
            UserId = "12345"
        });

        var expectedPost = new PostDto()
        {
            Username = "john.doe",
            Id = "P1",
            Post = "Post",
            Reactions = new List<string>() { "LIKE" },
            Tags = new List<string>() { "HISTORY" }
        };

        _dtoPosts.Add(expectedPost);

        _dtoPosts.Add(new PostDto()
        {
            Username = "rose.doe",
            Id = "P1",
            Post = "Post"
        });

        var result = _handler.Handle(new GetPostsUsersMasterCardRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<PostDto>>();
        result.ShouldHaveSingleItem();

        var firstResult = result.FirstOrDefault();
        firstResult.ShouldNotBeNull();

        firstResult.Username.ShouldBe(expectedPost.Username);
        firstResult.Id.ShouldBe(expectedPost.Id);
        firstResult.Post.ShouldBe(expectedPost.Post);
        firstResult.Reactions.ShouldBe(expectedPost.Reactions);
        firstResult.Tags.ShouldBe(expectedPost.Tags);
    }
}
