using MediatR;
namespace FirstApplication.Application.Features.GetFromDummyApiGrpc;

///<see cref="GetFromDummyApiGrpcRequestHandler.Handle(GetFromDummyApiGrpcRequest, CancellationToken)"/>
public class GetFromDummyApiGrpcRequest : IRequest<Unit>
{
}
