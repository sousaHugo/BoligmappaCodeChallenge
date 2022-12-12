using FirstApplication.Domain.Models;

namespace FirstApplication.Application.Contracts.Services.Grpc;

public interface IUserGrpcService
{
    Task<IEnumerable<UserGrpcModel>> GetAllUsersAsync(CancellationToken CancellationToken = default);
}
