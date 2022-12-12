using FirstApplication.Application.Dtos;
using FirstApplication.Application.Features.GetAllUserInformation;
using FirstApplication.Application.Features.GetFromDummyApi;
using FirstApplication.Application.Features.GetFromDummyApiAggregator;
using FirstApplication.Application.Features.GetFromDummyApiExtra;
using FirstApplication.Application.Features.GetFromDummyApiGrpc;
using FirstApplication.Application.Features.GetPostsUsersMasterCard;
using FirstApplication.Application.Features.GetTodosUsersMoreTwoPosts;
using FirstApplication.Application.Mapping;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FirstApplication.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
    {
        Services.AddRequests();
        
        Services.AddAutoMapper(cfg => cfg.AddProfile<ApplicationMappingProfile>());
        
        return Services;
    }
    private static IServiceCollection AddRequests(this IServiceCollection Services)
    {
        Services.AddScoped<IRequestHandler<GetFromDummyApiRequest, Unit>, GetFromDummyApiRequestHandler>();
        Services.AddScoped<IRequestHandler<GetFromDummyApiGrpcRequest, Unit>, GetFromDummyApiGrpcRequestHandler>();
        Services.AddScoped<IRequestHandler<GetFromDummyApiExtraRequest, Unit>, GetFromDummyApiExtraRequestHandler>();
        Services.AddScoped<IRequestHandler<GetFromDummyApiAggregatorRequest, Unit>, GetFromDummyApiAggregatorRequestHandler>();
        Services.AddScoped<IRequestHandler<GetPostsUsersMasterCardRequest, IEnumerable<PostDto>>, GetPostsUsersMasterCardRequestHandler>();
        Services.AddScoped<IRequestHandler<GetTodosUsersMoreTwoPostsRequest, IEnumerable<TodoDto>>, GetTodosUsersMoreTwoPostsRequestHandler>();
        Services.AddScoped<IRequestHandler<GetAllUserInformationRequest, IEnumerable<GetAllUserInfoDto>>, GetAllUserInformationRequestHandler>();

        return Services;
    }
}