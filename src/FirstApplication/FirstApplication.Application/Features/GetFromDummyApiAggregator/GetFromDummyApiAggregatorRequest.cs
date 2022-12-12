using MediatR;

namespace FirstApplication.Application.Features.GetFromDummyApiAggregator;

///<see cref="GetFromDummyApiAggregatorRequestHandler.Handle(GetFromDummyApiAggregatorRequest Request, CancellationToken CancellationToken)"/>
public class GetFromDummyApiAggregatorRequest : IRequest<Unit>
{
}
