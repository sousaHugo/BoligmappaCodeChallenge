using FirstApplication.Application.Dtos;
using MediatR;

namespace FirstApplication.Application.Features.GetAllUserInformation;

///<see cref="GetAllUserInformationRequestHandler.Handle(GetAllUserInformationRequest, CancellationToken)"/>
public class GetAllUserInformationRequest : IRequest<IEnumerable<GetAllUserInfoDto>>
{
}
