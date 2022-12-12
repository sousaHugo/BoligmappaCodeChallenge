using MediatR;

namespace FirstApplication.Application.Features.GetFromDummyApi;

///<see cref="GetFromDummyApiRequestHandler.Handle(GetFromDummyApiRequest Request, CancellationToken CancellationToken)"/>
public class GetFromDummyApiRequest : IRequest<Unit>
{
}
