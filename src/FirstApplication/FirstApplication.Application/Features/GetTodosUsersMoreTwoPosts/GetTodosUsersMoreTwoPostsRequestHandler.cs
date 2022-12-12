using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;

public class GetTodosUsersMoreTwoPostsRequestHandler : IRequestHandler<GetTodosUsersMoreTwoPostsRequest, IEnumerable<TodoDto>>
{
    private readonly ITodoService _todoService;
    private readonly IUserInfoRepository _userInfoRepository;

    private readonly ILogger<GetTodosUsersMoreTwoPostsRequestHandler> _handlerLogger;
    public GetTodosUsersMoreTwoPostsRequestHandler(ITodoService TodoService, IUserInfoRepository UserInfoRepository,
        ILogger<GetTodosUsersMoreTwoPostsRequestHandler> HandlerLogger)
    {
        _todoService = TodoService ?? throw new ArgumentNullException(nameof(TodoService));
        _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
    }

    /// <summary>
    /// This functionality retrieves all Users that have more than two Posts and that are stored in the DB. For each of these Users, a call is made to the Dummy Api, returning their Todos. For each registration, a request is also made to the Users API and if the User is found from Todos.
    /// </summary>
    /// <param name="GetTodosUsersMoreTwoPostsRequest">The request doesnt have any parameter field.</param>
    /// <returns>IEnumerable<TodoDto> - List of Todos wich the associated User has more than 2 Posts.</returns>

    public async Task<IEnumerable<TodoDto>> Handle(GetTodosUsersMoreTwoPostsRequest request, CancellationToken cancellationToken)
    {
        _handlerLogger.LogInformation("GetTodosUsersMoreTwoPostsRequestHandler Handle has started.");

        var returnResult = new List<TodoDto>();

        var userTodos = await _userInfoRepository.GetUserMoreTwoPostsAsync();

        foreach(var item in userTodos)
        {
            var todos = await _todoService.GetAllTodosByUserIdAsync<IEnumerable<TodoDto>>(item.UserId);
            returnResult.AddRange(todos);
        }

        _handlerLogger.LogInformation("GetTodosUsersMoreTwoPostsRequestHandler Handle has ended.");

        return returnResult;
    }
}
