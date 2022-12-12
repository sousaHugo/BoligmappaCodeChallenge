using MediatR;
using SecondApplication.Application.Dtos;

namespace SecondApplication.Application.Features.GetPostsFromDummyApiGRpc;

///<see cref="GetPostsFromDummyApiGRpcRequestHandler.Handle(SecondApplication.Application.Features.GetPostsFromDummyApiGRpc.GetPostsFromDummyApiGRpcRequest, CancellationToken)"/>
public class GetPostsFromDummyApiGRpcRequest : IRequest<IEnumerable<PostDto>>
{
}
