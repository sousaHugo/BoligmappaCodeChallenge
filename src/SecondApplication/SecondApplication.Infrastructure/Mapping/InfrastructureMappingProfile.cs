using AutoMapper;
using BCCP.DummyGrpc;
using SecondApplication.Domain.Models;

namespace SecondApplication.Infrastructure.Mapping
{
    internal class InfrastructureMappingProfile : Profile
    {
        public InfrastructureMappingProfile()
        {
            CreateMap<PostModel, PostGrpcModel>().ReverseMap();
        }
    }
}
