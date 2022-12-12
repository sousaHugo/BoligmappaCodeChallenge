using MediatR;
using SecondApplication.Application.Dtos;

namespace SecondApplication.Application.Features.GetAllPostInfo;

///<see cref="GetAllPostInfoRequestHandler.Handle(SecondApplication.Application.Features.GetAllPostInfo.GetAllPostInfoRequest, CancellationToken)"/>
public class GetAllPostInfoRequest : IRequest<IEnumerable<PostInfoDto>>
{
}
