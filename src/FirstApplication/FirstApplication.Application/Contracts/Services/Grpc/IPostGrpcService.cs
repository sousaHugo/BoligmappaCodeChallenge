using FirstApplication.Domain.Models;

namespace FirstApplication.Application.Contracts.Services.Grpc;

public interface IPostGrpcService
{
    Task<IEnumerable<PostGrpcModel>> GetAllPostsAsync(CancellationToken CancellationToken = default);
}
