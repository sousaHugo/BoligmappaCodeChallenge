using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FirstApplication.Application.Features.GetPostsUsersMasterCard;

public class GetPostsUsersMasterCardRequestHandler : IRequestHandler<GetPostsUsersMasterCardRequest, IEnumerable<PostDto>>
{
    private readonly IPostService _postService;
    private readonly IUserInfoRepository _userInfoRepository;

    private readonly ILogger<GetPostsUsersMasterCardRequestHandler> _handlerLogger;
    public GetPostsUsersMasterCardRequestHandler(IPostService PostService, IUserInfoRepository UserInfoRepository,
        ILogger<GetPostsUsersMasterCardRequestHandler> HandlerLogger)
    {
        _postService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        _userInfoRepository = UserInfoRepository ?? throw new ArgumentNullException(nameof(UserInfoRepository));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
    }

    public async Task<IEnumerable<PostDto>> Handle(GetPostsUsersMasterCardRequest Request, CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetPostsUsersMasterCardRequestHandler Handle has started.");

        var returnResult = new List<PostDto>();

        var userPosts = await _userInfoRepository.GetUserUseMasterCardAsync();

        foreach(var item in userPosts)
        {
            var posts = await _postService.GetAllPostsByUsernameAsync<IEnumerable<PostDto>>(item.Username);
            returnResult.AddRange(posts);
        }

        _handlerLogger.LogInformation("GetPostsUsersMasterCardRequestHandler Handle has ended.");

        return returnResult;
    }
}
