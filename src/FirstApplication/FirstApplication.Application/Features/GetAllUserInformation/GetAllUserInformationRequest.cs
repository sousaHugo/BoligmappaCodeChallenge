using FirstApplication.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApplication.Application.Features.GetAllUserInformation
{
    public class GetAllUserInformationRequest : IRequest<IEnumerable<GetAllUserInfoDto>>
    {
    }
}
