using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetFromDummyApi;
using FirstApplication.Application.Mapping;
using FirstApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features;

public class GetFromDummyApiTest
{
    private readonly GetFromDummyApiRequestHandler _handler;

    private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
    private readonly Mock<IPostService> _postServiceMock;
    private readonly Mock<ITodoService> _todoServiceMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<ILogger<GetFromDummyApiRequestHandler>> _loggerMock;
    private readonly IMapper _mapperMock;

    private readonly List<UserInfo> _dbUsers = new();
    private readonly List<PostDto> _dtoPosts = new();
    private readonly List<TodoDto> _dtoTodos = new();
    private readonly List<UserDto> _dtoUsers = new();

    private string _newUserInfoId;

    public GetFromDummyApiTest()
    {
        _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
        _postServiceMock = new Mock<IPostService>();
        _todoServiceMock = new Mock<ITodoService>();
        _userServiceMock = new Mock<IUserService>();
        _loggerMock = new Mock<ILogger<GetFromDummyApiRequestHandler>>();
        _mapperMock = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new ApplicationMappingProfile())));

        _userInfoRepositoryMock.Setup(m => m.GetByUserIdAsync(It.IsAny<string>())).ReturnsAsync((string UserId) => {

            return _dbUsers.SingleOrDefault(a => a.UserId == UserId);
        });
        _userInfoRepositoryMock.Setup(m => m.AddAsync(It.IsAny<UserInfo>())).ReturnsAsync((UserInfo User) => {

            User.Id = _newUserInfoId;
            _dbUsers.Add(User);

            return User;
        });
        _userInfoRepositoryMock.Setup(m => m.UpdateAsync(It.IsAny<UserInfo>())).Callback((UserInfo User) => {

            var post = _dbUsers.Single(a => a.UserId == User.UserId);
            var indexOf = _dbUsers.IndexOf(post);
            _dbUsers[indexOf] = User;

        });
        _postServiceMock.Setup(m => m.GetAllPostsAsync<IEnumerable<PostDto>>()).ReturnsAsync(() => _dtoPosts);
        _todoServiceMock.Setup(m => m.GetAllTodosAsync<IEnumerable<TodoDto>>()).ReturnsAsync(() => _dtoTodos);
        _userServiceMock.Setup(m => m.GetAllUsersAsync<IEnumerable<UserDto>>()).ReturnsAsync(() => _dtoUsers);

        _handler = new GetFromDummyApiRequestHandler(_postServiceMock.Object, _todoServiceMock.Object, _userServiceMock.Object, _userInfoRepositoryMock.Object,
            _mapperMock, _loggerMock.Object);
    }

    [Fact]
    public void GetFromDummyApiRequestHandler_AddPostSuccess()
    {
        var post = new PostDto()
        {
            Id = "1234",
            Post = "Teste",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" },
            Username = "john.doe"
        };

        _dtoPosts.Add(post);

        _dtoTodos.Add(new TodoDto()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "1234"
        });

        _dtoUsers.Add(new UserDto()
        {
            Id = "U2",
            CardType = CardType.MASTERCARD,
            Username = "john.doe"
        });

        _newUserInfoId = "1234";

        _ = _handler.Handle(new GetFromDummyApiRequest() { }, default).Result;

        _dbUsers.ShouldNotBeEmpty();
        _dbUsers.ShouldNotBeNull();
        _dtoPosts.Count.ShouldBe(_dbUsers.Count);

        var newUser = _dbUsers.FirstOrDefault();
        newUser.ShouldNotBeNull();

        newUser.Username.ShouldBe(post.Username);
        newUser.UseMasterCard.ShouldBeTrue();
        newUser.Id.ShouldBe(_newUserInfoId);

    }

    [Fact]
    public void GetFromDummyApiRequestHandler_AddPostFailValidation()
    {
        var post = new PostDto()
        {
            Id = "1234",
            Post = "Teste",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" },
            Username = "john.doe"
        };

        _dtoPosts.Add(post);

        _dtoTodos.Add(new TodoDto()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "1234"
        });

        _dtoUsers.Add(new UserDto()
        {
            CardType = CardType.MASTERCARD,
            Username = "john.doe"
        });

        _newUserInfoId = "1234";

        _ = _handler.Handle(new GetFromDummyApiRequest() { }, default).Result;

        _dbUsers.ShouldBeEmpty();
    }
    [Fact]
    public void GetPostsFromDummyApiRequestHandler_UpdatePostSuccess()
    {
        var userInfo = new UserInfo()
        {
            Id = "1234",
            Username = "john.doe",
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            UserId = "U1",
            UseMasterCard = true,
            NumberOfPosts = 1,
            NumberOfTodos = 1
        };

        _dbUsers.Add(userInfo);

        _dtoPosts.Add(new PostDto()
        {
            Id = "123455",
            Post = "Test 1",
            Tags = new List<string>() { "FICTION", "FRENCH", "HISTORY" },
            Reactions = new List<string>() { "LIKE" },
            Username = "john.doe"
        });

        _dtoPosts.Add(new PostDto()
        {
            Id = "1234556",
            Post = "Test 2",
            Username = "john.doe",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" }
        });

        _dtoPosts.Add(new PostDto()
        {
            Id = "1234556",
            Post = "Test 2",
            Username = "rose.doe",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" }
        });

        _dtoTodos.Add(new TodoDto()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "U1"
        });

        _dtoUsers.Add(new UserDto()
        {
            CardType = CardType.VISA,
            Username = "john.doe",
            Id = "U1"
        });

        _dtoUsers.Add(new UserDto()
        {
            CardType = CardType.VISA,
            Username = "rose.doe",
            Id = "U2"
        });

        _dtoUsers.Add(new UserDto()
        {
            CardType = CardType.MASTERCARD,
            Username = "rose.doe"
        });

        _ = _handler.Handle(new GetFromDummyApiRequest() { }, default).Result;


        var updated = _dbUsers.SingleOrDefault(a => a.UserId == userInfo.UserId);
        updated.ShouldNotBeNull();
        updated.UseMasterCard.ShouldBeFalse();
        updated.NumberOfTodos.ShouldBe(1);
        updated.NumberOfPosts.ShouldBe(2);

        var newUser = _dbUsers.SingleOrDefault(a => a.UserId == "U2");
        newUser.ShouldNotBeNull();
        newUser.UseMasterCard.ShouldBeFalse();
        newUser.NumberOfTodos.ShouldBe(0);
        newUser.NumberOfPosts.ShouldBe(1);
    }
}
