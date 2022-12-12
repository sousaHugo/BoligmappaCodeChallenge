using BCCP.Shared.Constants;
using BCCP.Shared.Extensions;
using BCCP.Shared.Repositories;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Application.Contracts.Services;
using FirstApplication.Application.Contracts.Services.Grpc;
using FirstApplication.Infrastructure.Contexts;
using FirstApplication.Infrastructure.Mapping;
using FirstApplication.Infrastructure.Repositories;
using FirstApplication.Infrastructure.Services;
using FirstApplication.Infrastructure.Services.Grpc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static BCCP.DummyGrpc.Posts;
using static BCCP.DummyGrpc.Todos;
using static BCCP.DummyGrpc.Users;

namespace FirstApplication.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddDatabase(Configuration);
        Services.AddHttpClients(Configuration);
        Services.AddRepositories();
        Services.AddServices();
        Services.AddGrpc(Configuration);
        Services.AddAutoMapper(cfg => cfg.AddProfile<InfrastructureMappingProfile>());

        return Services;
    }
    private static IServiceCollection AddDatabase(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddDatabaseContext<ApplicationDbContext>(Configuration);
        Services.MigrateDatabase<ApplicationDbContext>();

        return Services;
    }
    private static IServiceCollection AddHttpClients(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddHttpClient<IUserService, UserService>();
        Services.AddHttpClient<ITodoService, TodoService>();
        Services.AddHttpClient<IPostService, PostService>();

        BaseServiceConstants.DummyApiBase = Configuration["ApiUrls:DummyApiUrl"];
        BaseServiceConstants.DummyApiAggregatorBase = Configuration["ApiUrls:DummyApiAggregatorUrl"];

        return Services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection Services)
    {
        Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        Services.AddScoped<IUserInfoRepository, UserInfoRepository>();

        return Services;
    }
    private static IServiceCollection AddServices(this IServiceCollection Services)
    {
        Services.AddScoped<IPostService, PostService>();
        Services.AddScoped<IUserService, UserService>();
        Services.AddScoped<ITodoService, TodoService>();
        Services.AddScoped<IUserInfoService, UserInfoService>();

        return Services;
    }
    private static IServiceCollection AddGrpc(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddGrpcClient<PostsClient>(o =>
        {
            o.Address = new Uri(Configuration["ApiUrls:DummyGrpcUrl"]);
        });


        Services.AddGrpcClient<TodosClient>(o =>
        {
            o.Address = new Uri(Configuration["ApiUrls:DummyGrpcUrl"]);
        });


        Services.AddGrpcClient<UsersClient>(o =>
        {
            o.Address = new Uri(Configuration["ApiUrls:DummyGrpcUrl"]);
        });


        Services.AddScoped<IPostGrpcService, PostGrpcService>();
        Services.AddScoped<ITodoGrpService, TodoGrpcService>();
        Services.AddScoped<IUserGrpcService, UserGrpcService>();

        return Services;
    }
}
