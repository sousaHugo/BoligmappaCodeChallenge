using AutoMapper;
using BCCP.DummyGrpc;
using FirstApplication.Domain.Models;

namespace FirstApplication.Infrastructure.Mapping;

public class InfrastructureMappingProfile : Profile
{
    public InfrastructureMappingProfile()
    {
        CreateMap<PostModel, PostGrpcModel>().ReverseMap();
        CreateMap<UserModel, UserGrpcModel>().ReverseMap();
        CreateMap<TodoModel, TodoGrpcModel>().ReverseMap();
    }
}
