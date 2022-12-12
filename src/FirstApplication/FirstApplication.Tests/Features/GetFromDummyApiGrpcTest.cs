using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services.Grpc;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetFromDummyApiGrpc;
using FirstApplication.Application.Mapping;
using FirstApplication.Domain.Entities;
using FirstApplication.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features;

public class GetFromDummyApiGrpcTest
{
    private readonly GetFromDummyApiGrpcRequestHandler _handler;

    private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
    private readonly Mock<IPostGrpcService> _postServiceMock;
    private readonly Mock<ITodoGrpService> _todoServiceMock;
    private readonly Mock<IUserGrpcService> _userServiceMock;
    private readonly Mock<ILogger<GetFromDummyApiGrpcRequestHandler>> _loggerMock;
    private readonly IMapper _mapperMock;

    private readonly List<UserInfo> _dbUsers = new();
    private readonly List<PostGrpcModel> _modelPostsGrpc = new();
    private readonly List<TodoGrpcModel> _modelTodosGrpc = new();
    private readonly List<UserGrpcModel> _modelUsersGrpc = new();

    private string _newUserInfoId;

    public GetFromDummyApiGrpcTest()
    {
        _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
        _postServiceMock = new Mock<IPostGrpcService>();
        _todoServiceMock = new Mock<ITodoGrpService>();
        _userServiceMock = new Mock<IUserGrpcService>();
        _loggerMock = new Mock<ILogger<GetFromDummyApiGrpcRequestHandler>>();
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
        _postServiceMock.Setup(m => m.GetAllPostsAsync(default)).ReturnsAsync(() => _modelPostsGrpc);
        _todoServiceMock.Setup(m => m.GetAllTodosAsync(default)).ReturnsAsync(() => _modelTodosGrpc);
        _userServiceMock.Setup(m => m.GetAllUsersAsync(default)).ReturnsAsync(() => _modelUsersGrpc);


        _handler = new GetFromDummyApiGrpcRequestHandler(_postServiceMock.Object, _todoServiceMock.Object, _userServiceMock.Object, _userInfoRepositoryMock.Object,
            _mapperMock, _loggerMock.Object);
    }

    [Fact]
    public void GetFromDummyApiRequestHandler_AddPostSuccess()
    {
        var post = new PostGrpcModel()
        {
            Id = "1234",
            Post = "Teste",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" },
            Username = "john.doe"
        };

        _modelPostsGrpc.Add(post);

        _modelTodosGrpc.Add(new TodoGrpcModel()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "1234"
        });

        _modelUsersGrpc.Add(new UserGrpcModel()
        {
            Id = "U2",
            CardType = (int)CardType.MASTERCARD,
            Username = "john.doe"
        });

        _newUserInfoId = "1234";

        _ = _handler.Handle(new GetFromDummyApiGrpcRequest() { }, default).Result;

        _dbUsers.ShouldNotBeEmpty();
        _dbUsers.ShouldNotBeNull();
        _modelPostsGrpc.Count.ShouldBe(_dbUsers.Count);

        var newUser = _dbUsers.FirstOrDefault();
        newUser.ShouldNotBeNull();

        newUser.Username.ShouldBe(post.Username);
        newUser.UseMasterCard.ShouldBeTrue();
        newUser.Id.ShouldBe(_newUserInfoId);

    }

    [Fact]
    public void GetFromDummyApiRequestHandler_AddPostFailValidation()
    {
        var post = new PostGrpcModel()
        {
            Id = "1234",
            Post = "Teste",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" },
            Username = "john.doe"
        };

        _modelPostsGrpc.Add(post);

        _modelTodosGrpc.Add(new TodoGrpcModel()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "1234"
        });

        _modelUsersGrpc.Add(new UserGrpcModel()
        {
            CardType = (int)CardType.MASTERCARD,
            Username = "john.doe"
        });

        _newUserInfoId = "1234";

        _ = _handler.Handle(new GetFromDummyApiGrpcRequest() { }, default).Result;

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

        _modelPostsGrpc.Add(new PostGrpcModel()
        {
            Id = "123455",
            Post = "Test 1",
            Tags = new List<string>() { "FICTION", "FRENCH", "HISTORY" },
            Reactions = new List<string>() { "LIKE" },
            Username = "john.doe"
        });

        _modelPostsGrpc.Add(new PostGrpcModel()
        {
            Id = "1234556",
            Post = "Test 2",
            Username = "john.doe",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" }
        });

        _modelPostsGrpc.Add(new PostGrpcModel()
        {
            Id = "1234556",
            Post = "Test 2",
            Username = "rose.doe",
            Tags = new List<string>() { "HISTORY" },
            Reactions = new List<string>() { "SMILE" }
        });

        _modelTodosGrpc.Add(new TodoGrpcModel()
        {
            Description = "Description",
            Id = "1",
            Title = "Title Todo",
            UserId = "U1"
        });

        _modelUsersGrpc.Add(new UserGrpcModel()
        {
            CardType = (int)CardType.VISA,
            Username = "john.doe",
            Id = "U1"
        });

        _modelUsersGrpc.Add(new UserGrpcModel()
        {
            CardType = (int)CardType.VISA,
            Username = "rose.doe",
            Id = "U2"
        });

        _modelUsersGrpc.Add(new UserGrpcModel()
        {
            CardType = (int)CardType.MASTERCARD,
            Username = "rose.doe"
        });

        _ = _handler.Handle(new GetFromDummyApiGrpcRequest() { }, default).Result;


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
