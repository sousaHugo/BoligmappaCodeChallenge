using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Dtos.Validator;
using FirstApplication.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirstApplication.Application.Features.GetFromDummyApi;

public class GetFromDummyApiRequestHandler : IRequestHandler<GetFromDummyApiRequest, Unit>
{
    private readonly IPostService _postService;
    private readonly ITodoService _todoService;
    private readonly IUserService _userService;
    private readonly IUserInfoRepository _userInfoRepository;

    private readonly IMapper _mapper;
    private readonly ILogger<GetFromDummyApiRequestHandler> _handlerLogger;
    
    public GetFromDummyApiRequestHandler(IPostService PostService, ITodoService TodoService, IUserService UserService,
        IUserInfoRepository UserInfoRepository, IMapper Mapper, ILogger<GetFromDummyApiRequestHandler> HandlerLogger)
    {
        _postService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        _todoService = TodoService ?? throw new ArgumentNullException(nameof(TodoService));
        _userService = UserService ?? throw new ArgumentNullException(nameof(UserService));
        _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
    }

    /// <summary>
    ///  As I don’t know which methods are provided by the Dummy Api, this functionality assumes that the existing operations only return all the records (Users, Todos, Posts).
    /// </summary>
    /// <param name="GetFromDummyApiRequest">The request doesnt have any parameter field.</param>
    public async Task<Unit> Handle(GetFromDummyApiRequest Request, 
        CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetFromDummyApiRequestHandler Handle has started.");

        var dummyFilteredPosts = await GetFilteredDummyPosts();

        if (dummyFilteredPosts.Any())
        {
            var dummyTodos = await _todoService.GetAllTodosAsync<IEnumerable<TodoDto>>();
            var dummyUsers = await _userService.GetAllUsersAsync<IEnumerable<UserDto>>();

            foreach (var dummyPost in dummyFilteredPosts)
            {
                var postUser = dummyUsers.FirstOrDefault(a => a.Username == dummyPost.Key);

                var userInformationDto = new UserInfoDto()
                {
                    NumberOfPosts = dummyPost.Count(),
                    NumberOfTodos = postUser != null ? dummyTodos.Count(t => t.UserId == postUser.Id) : 0,
                    UseMasterCard = postUser?.CardType == CardType.MASTERCARD,
                    UserId = postUser?.Id,
                    Username = postUser?.Username
                };

                if (UserInfoIsValid(userInformationDto))
                {
                    await UserInfoAddOrUpdate(userInformationDto);
                }
            }
        }
        else
        {
            _handlerLogger.LogInformation("No Posts were found to store.");
        }           

        _handlerLogger.LogInformation("GetFromDummyApiRequestHandler Handle has ended.");

        return Unit.Value;
    }
    
    private async Task<IEnumerable<IGrouping<string, PostDto>>> GetFilteredDummyPosts()
    {
        var dummyPosts = await _postService.GetAllPostsAsync<IEnumerable<PostDto>>();

        return dummyPosts.Where(p => p.Reactions != null && p.Reactions.Any()  && p.Tags != null && p.Tags.Any(r => r == "HISTORY"))
            .GroupBy(a => a.Username)
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
