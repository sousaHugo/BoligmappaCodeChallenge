using AutoMapper;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using FirstApplication.Application.Dtos.Validator;
using MediatR;
using Microsoft.Extensions.Logging;
using FirstApplication.Domain.Entities;

namespace FirstApplication.Application.Features.GetFromDummyApiExtra
{
    public class GetFromDummyApiExtraRequestHandler : IRequestHandler<GetFromDummyApiExtraRequest, Unit>
    {
        private readonly IPostService _postService;
        private readonly ITodoService _todoService;
        private readonly IUserService _userService;
        private readonly IUserInfoRepository _userInfoRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<GetFromDummyApiExtraRequestHandler> _handlerLogger;

        public GetFromDummyApiExtraRequestHandler(IPostService PostService, ITodoService TodoService, IUserService UserService,
            IUserInfoRepository UserInfoRepository, IMapper Mapper, ILogger<GetFromDummyApiExtraRequestHandler> HandlerLogger)
        {
            _postService = PostService ?? throw new ArgumentNullException(nameof(PostService));
            _todoService = TodoService ?? throw new ArgumentNullException(nameof(TodoService));
            _userService = UserService ?? throw new ArgumentNullException(nameof(UserService));
            _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
            _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
            _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
        }

        public async Task<Unit> Handle(GetFromDummyApiExtraRequest request, CancellationToken cancellationToken)
        {
            _handlerLogger.LogInformation("GetFromDummyApiExtraRequestHandler Handle has started.");

            var dummyFilteredPosts = await GetFilteredDummyPosts();

            foreach (var dummyPost in dummyFilteredPosts)
            {
                var dummyUser = await _userService.GetAllUsersByUsernameAsync<UserDto>(dummyPost.Key) ?? null;
                
                IEnumerable<TodoDto> dummyTodos = new List<TodoDto>();

                if (dummyUser != null)
                    dummyTodos = await _todoService.GetAllTodosByUserIdAsync<IEnumerable<TodoDto>>(dummyUser.Id);


                var userInformationDto = new UserInfoDto()
                {
                    NumberOfPosts = dummyPost.Count(),
                    NumberOfTodos = dummyTodos.Count(t => t.UserId == dummyUser.Id),
                    UseMasterCard = dummyUser?.CardType == CardType.MASTERCARD,
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

            _handlerLogger.LogInformation("GetFromDummyApiExtraRequestHandler Handle has ended.");

            return Unit.Value;
        }
        private async Task<IEnumerable<IGrouping<string, PostDto>>> GetFilteredDummyPosts()
        {
            var dummyPosts = await _postService.GetAllPostsByTagAsync<IEnumerable<PostDto>>("HISTORY");
            
            return dummyPosts.Where(a => a.Reactions.Any()).GroupBy(a => a.Username)
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
}
