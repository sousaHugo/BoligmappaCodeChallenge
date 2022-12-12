using SecondApplication.Domain.Models;

namespace SecondApplication.Application.Contracts.Services.Grpc;

public interface IPostGrpcService
{
    /// <summary>
    /// This method is responsible for getting the Post Information from the dummy gRPC.
    /// </summary>
    /// <returns>T - Type wich must be used to convert the information from the dummy gRPC.</returns>
    Task<IEnumerable<PostGrpcModel>> GetAllPostsAsync(CancellationToken CancellationToken = default);
}
