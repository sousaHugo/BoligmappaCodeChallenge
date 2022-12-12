using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services.Grpc;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Dtos.Validator;
using FirstApplication.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirstApplication.Application.Features.GetFromDummyApiGrpc;

public class GetFromDummyApiGrpcRequestHandler : IRequestHandler<GetFromDummyApiGrpcRequest, Unit>
{
    private readonly IPostGrpcService _postGrpcService;
    private readonly ITodoGrpService _todoGrpcService;
    private readonly IUserGrpcService _userGrpcService;
    private readonly IUserInfoRepository _userInfoRepository;

    private readonly IMapper _mapper;
    private readonly ILogger<GetFromDummyApiGrpcRequestHandler> _handlerLogger;

    public GetFromDummyApiGrpcRequestHandler(IPostGrpcService PostService, ITodoGrpService TodoService, IUserGrpcService UserService,
       IUserInfoRepository UserInfoRepository, IMapper Mapper, ILogger<GetFromDummyApiGrpcRequestHandler> HandlerLogger)
    {
        _postGrpcService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        _todoGrpcService = TodoService ?? throw new ArgumentNullException(nameof(TodoService));
        _userGrpcService = UserService ?? throw new ArgumentNullException(nameof(UserService));
        _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
    }

    /// <summary>
    /// I decided to also implement a Feature similar to GetFromDummyApi but in which the calls are made to the gRPC server.
    /// </summary>
    /// <param name="GetFromDummyApiGrpcRequest">The request doesnt have any parameter field.</param>
    public async Task<Unit> Handle(GetFromDummyApiGrpcRequest Request, CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetFromDummyApiGrpcRequest Handle has started.");

        var dummyFilteredPosts = await GetFilteredDummyPosts();
        var dummyTodos = await _todoGrpcService.GetAllTodosAsync(CancellationToken: CancellationToken);
        var dummyUsers = await _userGrpcService.GetAllUsersAsync(CancellationToken: CancellationToken);

        foreach (var dummyPost in dummyFilteredPosts)
        {
            var dummyUser = dummyUsers.FirstOrDefault(a => a.Username == dummyPost.Key) ?? null;

            var userInformationDto = new UserInfoDto()
            {
                NumberOfPosts = dummyPost.Count(),
                NumberOfTodos = dummyTodos.Count(t => t.UserId == dummyUser.Id),
                UseMasterCard = dummyUser?.CardType == (int)CardType.MASTERCARD,
                UserId = dummyUser?.Id,
                Username = dummyUser?.Username
            };

            if (UserInfoIsValid(userInformationDto))
            {
                await UserInfoAddOrUpdate(userInformationDto);
            }
        }

        if (!dummyFilteredPosts.Any())
            _handlerLogger.LogInformation("No Posts were found to store.");

        _handlerLogger.LogInformation("GetFromDummyApiGrpcRequest Handle has ended.");

        return Unit.Value;
    }
    private async Task<IEnumerable<IGrouping<string, PostDto>>> GetFilteredDummyPosts()
    {
        var dummyPosts = await _postGrpcService.GetAllPostsAsync();

        return dummyPosts.Where(p => p.Reactions != null && p.Reactions.Any()
            && p.Tags != null && p.Tags.Any(r => r == "HISTORY"))
            .Select(a => _mapper.Map<PostDto>(a)).GroupBy(a => a.Username)
            .ToList();
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
