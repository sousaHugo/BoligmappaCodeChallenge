using FirstApplication.Application.Dtos;
using MediatR;

namespace FirstApplication.Application.Features.GetPostsUsersMasterCard;

public class GetPostsUsersMasterCardRequest : IRequest<IEnumerable<PostDto>>
{
}
