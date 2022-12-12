using MediatR;

namespace SecondApplication.Application.Features.GetPostsFromDummyApi;

///<see cref="GetPostsFromDummyApiRequestHandler.Handle(SecondApplication.Application.Features.GetPostsFromDummyApi.GetPostsFromDummyApiRequest, CancellationToken)"/>
public class GetPostsFromDummyApiRequest : IRequest<Unit>
{
}
