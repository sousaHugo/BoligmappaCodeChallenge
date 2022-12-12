using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Dtos.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using FirstApplication.Domain.Entities;
using FirstApplication.Application.Dtos.Extensions;

namespace FirstApplication.Application.Features.GetFromDummyApiAggregator;

public class GetFromDummyApiAggregatorRequestHandler : IRequestHandler<GetFromDummyApiAggregatorRequest, Unit>
{
    private readonly IUserInfoService _userInfoService;
    private readonly IUserInfoRepository _userInfoRepository;

    private readonly ILogger<GetFromDummyApiAggregatorRequestHandler> _handlerLogger;
    private readonly IMapper _mapper;

    public GetFromDummyApiAggregatorRequestHandler(IUserInfoService UserInfoService, IUserInfoRepository UserInfoRepository,
        ILogger<GetFromDummyApiAggregatorRequestHandler> HandlerLogger, IMapper Mapper)
    {
        _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
        _userInfoService = UserInfoService ?? throw new ArgumentNullException(nameof(UserInfoService));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }
    public async Task<Unit> Handle(GetFromDummyApiAggregatorRequest Request, 
        CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetFromDummyApiAggregatorRequestHandler Handle has started.");

        var dummyUserInfo = await _userInfoService.GetAllUserInfoAsync<IEnumerable<UserInfoAggregatorDto>>("HISTORY");

        var dummyUserInfoDto = dummyUserInfo.AsUserInfoDto();

        foreach (var userInfo in dummyUserInfoDto)
        {
            if (UserInfoIsValid(userInfo))
            {
                await UserInfoAddOrUpdate(userInfo);
            }
        }

        _handlerLogger.LogInformation("GetFromDummyApiAggregatorRequestHandler Handle has ended.");
        return Unit.Value;
    }
    private bool UserInfoIsValid(UserInfoDto UserInfoDto)
    {
        var validator = new UserInformationValidator();
        var validationResult = validator.Validate(UserInfoDto);

        if (validationResult.IsValid)
            return true;

        _handlerLogger.LogError($"{nameof(UserInfoDto)}: {UserInfoDto.Username} is not valid. Errors: {validationResult.Errors.Select(a => a.ErrorMessage)}");
        return false;
    }
    private async Task UserInfoAddOrUpdate(UserInfoDto UserInfo)
    {
        var userPostEf = await _userInfoRepository.GetByUserIdAsync(UserInfo.UserId);

        if (userPostEf is not null)
        {
            userPostEf.UseMasterCard = UserInfo.UseMasterCard;
            userPostEf.NumberOfPosts = UserInfo.NumberOfPosts;
            userPostEf.NumberOfTodos = UserInfo.NumberOfTodos;

            await _userInfoRepository.UpdateAsync(userPostEf);
            _handlerLogger.LogInformation($"User: {userPostEf.UserId} has been updated.");
        }
        else
        {
            userPostEf = _mapper.Map<UserInfo>(UserInfo);
            userPostEf = await _userInfoRepository.AddAsync(userPostEf);
            _handlerLogger.LogInformation($"User: {userPostEf.UserId} has been stored with Id: {userPostEf.Id}.");
        }
    }
}
