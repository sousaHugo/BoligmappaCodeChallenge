
using AutoMapper;
using SecondApplication.Application.Dtos;
using SecondApplication.Domain.Entities;
using SecondApplication.Domain.Models;

namespace SecondApplication.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<PostInfo, PostInfoDto>().ReverseMap();
            CreateMap<PostGrpcModel, PostDto>();
        }
    }
}
