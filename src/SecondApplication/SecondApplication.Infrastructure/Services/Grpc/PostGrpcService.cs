using AutoMapper;
using SecondApplication.Application.Contracts.Services.Grpc;
using SecondApplication.Domain.Models;
using static BCCP.DummyGrpc.Posts;

namespace SecondApplication.Infrastructure.Services.Grpc;

public class PostGrpcService : IPostGrpcService
{
    private readonly PostsClient _postClient;
    private readonly IMapper _mapper;
    public PostGrpcService(PostsClient PostClient, IMapper Mapper)
    {
        _postClient = PostClient ?? throw new ArgumentNullException(nameof(PostClient));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }

    public async Task<IEnumerable<PostGrpcModel>> GetAllPostsAsync(CancellationToken CancellationToken = default)
    {
        var response = await _postClient.GetAsync(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: CancellationToken);

        return _mapper.Map<IEnumerable<PostGrpcModel>>(response.Posts);
    }
}
