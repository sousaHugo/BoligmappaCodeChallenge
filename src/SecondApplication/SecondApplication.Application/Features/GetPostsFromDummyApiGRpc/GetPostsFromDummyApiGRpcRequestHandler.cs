using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SecondApplication.Application.Contracts.Services.Grpc;
using SecondApplication.Application.Dtos;

namespace SecondApplication.Application.Features.GetPostsFromDummyApiGRpc;

public class GetPostsFromDummyApiGRpcRequestHandler : IRequestHandler<GetPostsFromDummyApiGRpcRequest, IEnumerable<PostDto>>
{
    private readonly IPostGrpcService _postService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetPostsFromDummyApiGRpcRequestHandler> _handlerLogger;

    public GetPostsFromDummyApiGRpcRequestHandler(IPostGrpcService PostService, IMapper Mapper,
        ILogger<GetPostsFromDummyApiGRpcRequestHandler> HandlerLogger)
    {
        _postService = PostService ?? throw new ArgumentNullException(nameof(PostService));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
        _handlerLogger = HandlerLogger ?? throw new ArgumentNullException(nameof(HandlerLogger));
    }

    /// <summary>
    /// This method is responsible for getting the Post Information from the gRPC server.
    /// </summary>
    /// <param name="GetPostsFromDummyApiGRpcRequest">The request doesnt have any parameter field.</param>
    /// <returns>IEnumerable<PostDto> - All the Posts stored in the gRPC server.</returns
    public async Task<IEnumerable<PostDto>> Handle(GetPostsFromDummyApiGRpcRequest Request, 
        CancellationToken CancellationToken)
    {
        _handlerLogger.LogInformation("GetPostsFromDummyApiGRpcRequestHandler Handle has started.");

        var postsModel = await _postService.GetAllPostsAsync(CancellationToken);

        var postsDto = postsModel.Select(p => _mapper.Map<PostDto>(p));

        _handlerLogger.LogInformation("GetPostsFromDummyApiGRpcRequestHandler Handle has ended.");

        return postsDto;
    }
}
