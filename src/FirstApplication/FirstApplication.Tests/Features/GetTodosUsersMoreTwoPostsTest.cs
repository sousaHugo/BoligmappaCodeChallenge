using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;
using FirstApplication.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FirstApplication.Tests.Features;

public  class GetTodosUsersMoreTwoPostsTest
{
    private readonly GetTodosUsersMoreTwoPostsRequestHandler _handler;

    private readonly Mock<ITodoService> _todoServiceMock;
    private readonly Mock<IUserInfoRepository> _userInfoRepositoryMock;
    private readonly Mock<ILogger<GetTodosUsersMoreTwoPostsRequestHandler>> _loggerMock;

    private readonly List<TodoDto> _dtoTodos = new();
    private readonly List<UserInfo> _dbUsers = new();

    public GetTodosUsersMoreTwoPostsTest()
    {
        _userInfoRepositoryMock = new Mock<IUserInfoRepository>();
        _todoServiceMock = new Mock<ITodoService>();
        _loggerMock = new Mock<ILogger<GetTodosUsersMoreTwoPostsRequestHandler>>();

        _userInfoRepositoryMock.Setup(m => m.GetUserMoreTwoPostsAsync()).ReturnsAsync(() => {
            return _dbUsers.Where(a => a.NumberOfPosts > 2);
        
        });
        _todoServiceMock.Setup(m => m.GetAllTodosByUserIdAsync<IEnumerable<TodoDto>>(It.IsAny<string>())).ReturnsAsync((string UserId) => {

            return _dtoTodos.Where(a => a.UserId == UserId);
        });

        _handler = new GetTodosUsersMoreTwoPostsRequestHandler(_todoServiceMock.Object, _userInfoRepositoryMock.Object, _loggerMock.Object);
    }
    [Fact]
    public void GetTodosUsersMoreTwoPostsRequestHandler_NoResultsSuccess()
    {
        var result = _handler.Handle(new GetTodosUsersMoreTwoPostsRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<TodoDto>>();
        result.ShouldBeEmpty();
    }
    [Fact]
    public void GetTodosUsersMoreTwoPostsRequestHandler_OneResultSuccess()
    {
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "john.doe",
            NumberOfPosts = 3,
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

        _dtoTodos.Add(new TodoDto()
        {
            Id = "P1",
            Description = "TodoDescription",
            Title = "Ttile of Todo",
            UserId = "1234"
        });
        _dtoTodos.Add(new TodoDto()
        {
            Id = "P1",
            Description = "TodoDescription",
            Title = "Ttile of Todo",
            UserId = "123456"
        });

        var result = _handler.Handle(new GetTodosUsersMoreTwoPostsRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<TodoDto>>();
        result.ShouldHaveSingleItem();
    }

    [Fact]
    public void GetTodosUsersMoreTwoPostsRequestHandler_OneResultEqualInfoSuccess()
    {
        _dbUsers.Add(new UserInfo
        {
            CreatedDate = DateTime.Now,
            Id = Guid.NewGuid().ToString(),
            ModifiedDate = DateTime.Now,
            Username = "john.doe",
            NumberOfPosts = 3,
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

        var expectedTodo = new TodoDto()
        {
            Id = "P1",
            Description = "TodoDescription",
            Title = "Ttile of Todo",
            UserId = "1234"
        };

        _dtoTodos.Add(expectedTodo);
        _dtoTodos.Add(new TodoDto()
        {
            Id = "P1",
            Description = "TodoDescription",
            Title = "Ttile of Todo",
            UserId = "123456"
        });

        var result = _handler.Handle(new GetTodosUsersMoreTwoPostsRequest() { }, default).Result;

        result.ShouldBeAssignableTo<IEnumerable<TodoDto>>();
        result.ShouldHaveSingleItem();

        var firstResult = result.FirstOrDefault();
        firstResult.ShouldNotBeNull();

        firstResult.Description.ShouldBe(expectedTodo.Description);
        firstResult.Id.ShouldBe(expectedTodo.Id);
        firstResult.Title.ShouldBe(expectedTodo.Title);
        firstResult.UserId.ShouldBe(expectedTodo.UserId);
    }
}
