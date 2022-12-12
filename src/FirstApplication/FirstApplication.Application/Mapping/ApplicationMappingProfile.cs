using AutoMapper;
using FirstApplication.Application.Dtos;
using FirstApplication.Domain.Entities;
using FirstApplication.Domain.Models;

namespace FirstApplication.Application.Mapping;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<UserInfo, GetAllUserInfoDto>().ReverseMap();
        CreateMap<UserInfo, UserInfoDto>().ReverseMap();
        CreateMap<PostGrpcModel, PostDto>();
    }
}
