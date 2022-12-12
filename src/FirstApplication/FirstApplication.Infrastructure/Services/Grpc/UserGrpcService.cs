using AutoMapper;
using FirstApplication.Application.Contracts.Services.Grpc;
using FirstApplication.Domain.Models;
using static BCCP.DummyGrpc.Todos;
using static BCCP.DummyGrpc.Users;

namespace FirstApplication.Infrastructure.Services.Grpc;

public class UserGrpcService : IUserGrpcService
{
    private readonly UsersClient _usersClient;
    private readonly IMapper _mapper;
    public UserGrpcService(UsersClient UsersClient, IMapper Mapper)
    {
        _usersClient = UsersClient ?? throw new ArgumentNullException(nameof(UsersClient));
        _mapper = Mapper ?? throw new ArgumentNullException(nameof(Mapper));
    }

    public async Task<IEnumerable<UserGrpcModel>> GetAllUsersAsync(CancellationToken CancellationToken = default)
    {
        var response = await _usersClient.GetAsync(new Google.Protobuf.WellKnownTypes.Empty(), cancellationToken: CancellationToken);

        return _mapper.Map<IEnumerable<UserGrpcModel>>(response.Users);
    }
}
