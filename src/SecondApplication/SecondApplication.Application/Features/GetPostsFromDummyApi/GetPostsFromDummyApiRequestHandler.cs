using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Application.Contracts.Services;
using SecondApplication.Application.Dtos;
using SecondApplication.Application.Dtos.Extensions;
using SecondApplication.Application.Dtos.Validators;
using SecondApplication.Domain.Entities;

namespace SecondApplication.Application.Features.GetPostsFromDummyApi;

public class GetPostsFromDummyApiRequestHandler : IRequestHandler<GetPostsFromDummyApiRequest, Unit>
{
    private readonly IPostService _postService;
    private readonly IPostInfoRepository _postInfoRepository;
    private readonly ILogger<GetPostsFromDummyApiRequestHandler> _handlerLogger;
    private readonly IMapper _mapper;

    public GetPostsFromDummyApiRequestHandler(IPostService PostService, IPostInfoRepository PostInfoRepository,
        ILogger<GetPostsFromDummyApiRequestHandler> HandlerLogger, IMapper Mapper)
    {
        _postService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        _postInfoRepository = PostInfoRepository ?? throw new ArgumentNullException(nameof(PostInfoRepository));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }

    /// <summary>
    /// This method is responsible for getting the Post Information from the Dummy Api, validate if the Information is valid to store
    /// and then store (Add if not exists or Update if exists) in the database.
    /// </summary>
    /// <param name="GetPostsFromDummyApiRequest">The request doesnt have any parameter field.</param>
    /// <returns>Unit - Nothing to return.</returns
    public async Task<Unit> Handle(GetPostsFromDummyApiRequest Request, 
        CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetPostsFromDummyApiRequestHandler Handle has started.");

        var dummyPosts = await _postService.GetAllPostsAsync<IEnumerable<PostDto>>();
        var postInfoList = dummyPosts.AsPostInfoDto();

        foreach(var postInfo in postInfoList)
        {
            var isValid = PostInfoIsValid(postInfo);

            if (isValid)
            {
                await PostInfoAddOrUpdate(postInfo);
            }
        }

        _handlerLogger.LogInformation("GetPostsFromDummyApiRequestHandler Handle has ended.");

        return Unit.Value;
    }
    /// <summary>
    /// This method  Validate if the Information is valid to store in the database.
    /// </summary>
    /// <param name="PostInfoDto">Post information to validate.</param>
    /// <returns>Bool - Valid or Not.</returns
    private bool PostInfoIsValid(PostInfoDto PostInfoDto)
    {
        var validator = new PostInfoDtoValidator();
        var validationResult = validator.Validate(PostInfoDto);

        if (validationResult.IsValid)
            return true;

        _handlerLogger.LogError($"{nameof(PostInfoDto)}: {PostInfoDto.PostId} is not valid. Errors: {validationResult.Errors.Select(a => a.ErrorMessage)}");
        return false;
    }

    /// <summary>
    /// This method is responsible storing the information (Add if not exists or Update if exists) in the database.
    /// </summary>
    /// <param name="PostInfoDto">Post information to store</param>
    private async Task PostInfoAddOrUpdate(PostInfoDto PostInfoDto)
    {
        var postInfoEf = await _postInfoRepository.GetPostInfoByPostIdAsync(PostInfoDto.PostId);

        if (postInfoEf is not null)
        {
            postInfoEf.Username = PostInfoDto.Username;
            postInfoEf.HasFrenchTag = PostInfoDto.HasFrenchTag;
            postInfoEf.HasMoreThanTwoReactions = PostInfoDto.HasMoreThanTwoReactions;
            postInfoEf.HasFictonTag = PostInfoDto.HasFictonTag;

            await _postInfoRepository.UpdateAsync(postInfoEf);
            _handlerLogger.LogInformation($"Post: {PostInfoDto.PostId} has been updated.");
        }
        else
        {
            postInfoEf = _mapper.Map<PostInfo>(PostInfoDto);

            postInfoEf = await _postInfoRepository.AddAsync(postInfoEf);

            _handlerLogger.LogInformation($"Post: {postInfoEf.PostId} has been stored.");
        }
    }
}
