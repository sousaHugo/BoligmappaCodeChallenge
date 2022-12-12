using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SecondApplication.Application.Dtos;
using SecondApplication.Application.Features.GetAllPostInfo;
using SecondApplication.Application.Features.GetPostsFromDummyApi;
using SecondApplication.Application.Features.GetPostsFromDummyApiGRpc;
using SecondApplication.Application.Mapping;

namespace SecondApplication.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
    {
        Services.AddScoped<IRequestHandler<GetPostsFromDummyApiRequest, Unit>, GetPostsFromDummyApiRequestHandler>();
        Services.AddScoped<IRequestHandler<GetAllPostInfoRequest, IEnumerable<PostInfoDto>>, GetAllPostInfoRequestHandler>();
        Services.AddScoped<IRequestHandler<GetPostsFromDummyApiGRpcRequest, IEnumerable<PostDto>>, GetPostsFromDummyApiGRpcRequestHandler>();
        Services.AddAutoMapper(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        return Services;
    }
}
