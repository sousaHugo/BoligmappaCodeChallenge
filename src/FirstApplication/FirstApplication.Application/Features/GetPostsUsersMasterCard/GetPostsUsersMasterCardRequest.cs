using FirstApplication.Application.Dtos;
using MediatR;

namespace FirstApplication.Application.Features.GetPostsUsersMasterCard;

///<see cref="GetPostsUsersMasterCardRequestHandler.Handle(GetPostsUsersMasterCardRequest, CancellationToken)"/>
public class GetPostsUsersMasterCardRequest : IRequest<IEnumerable<PostDto>>
{
}
