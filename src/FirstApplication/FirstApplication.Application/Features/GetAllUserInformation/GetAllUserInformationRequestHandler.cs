using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirstApplication.Application.Features.GetAllUserInformation
{
    public class GetAllUserInformationRequestHandler : IRequestHandler<GetAllUserInformationRequest, IEnumerable<GetAllUserInfoDto>>
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUserInformationRequestHandler> _handlerLogger;

        public GetAllUserInformationRequestHandler(IUserInfoRepository UserInfoRepository, IMapper Mapper,
            ILogger<GetAllUserInformationRequestHandler> HandlerLogger)
        {
            _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
            _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
        }
        public async Task<IEnumerable<GetAllUserInfoDto>> Handle(GetAllUserInformationRequest Request, 
            CancellationToken CancellationToken)
        {
            _handlerLogger.LogInformation("GetAllUserInformationRequestHandler Handle has started.");

            var userInformationEf = await _userInfoRepository.GetAllAsync();

            var usersDtos = userInformationEf.Select(a => _mapper.Map<GetAllUserInfoDto>(a));

            _handlerLogger.LogInformation("GetAllUserInformationRequestHandler Handle has ended.");

            return usersDtos;
        }
    }
}
