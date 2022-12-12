using FirstApplication.Application.Dtos;
using MediatR;

namespace FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;

///<see cref="GetTodosUsersMoreTwoPostsRequestHandler.Handle(GetTodosUsersMoreTwoPostsRequest, CancellationToken)"/>
public class GetTodosUsersMoreTwoPostsRequest : IRequest<IEnumerable<TodoDto>>
{
}
