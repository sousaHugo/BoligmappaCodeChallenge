using FirstApplication.Domain.Models;

namespace FirstApplication.Application.Contracts.Services.Grpc;

public interface ITodoGrpService
{
    Task<IEnumerable<TodoGrpcModel>> GetAllTodosAsync(CancellationToken CancellationToken = default);
}
