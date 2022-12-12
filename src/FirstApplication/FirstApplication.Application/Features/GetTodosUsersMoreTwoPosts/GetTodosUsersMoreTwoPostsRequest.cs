using FirstApplication.Application.Dtos;
using MediatR;

namespace FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;

public class GetTodosUsersMoreTwoPostsRequest : IRequest<IEnumerable<TodoDto>>
{
}
