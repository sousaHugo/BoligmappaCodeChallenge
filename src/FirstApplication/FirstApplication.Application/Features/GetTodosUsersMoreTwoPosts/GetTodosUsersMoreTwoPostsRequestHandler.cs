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
