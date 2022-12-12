using AutoMapper;
using FirstApplication.Application.Contracts.Services.Grpc;
using FirstApplication.Domain.Models;
using static BCCP.DummyGrpc.Todos;

namespace FirstApplication.Infrastructure.Services.Grpc;

public class TodoGrpcService : ITodoGrpService
{
    private readonly TodosClient _todosClient;
    private readonly IMapper _mapper;
    public TodoGrpcService(TodosClient TodosClient, IMapper Mapper)
    {
        _todosClient = TodosClient ?? throw new ArgumentNullException(nameof(TodosClient));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }

    public async Task<IEnumerable<TodoGrpcModel>> GetAllTodosAsync(CancellationToken CancellationToken = default)
    {
        var response = await _todosClient.GetAsync(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: CancellationToken);

        return _mapper.Map<IEnumerable<TodoGrpcModel>>(response.Todos);
    }
}
