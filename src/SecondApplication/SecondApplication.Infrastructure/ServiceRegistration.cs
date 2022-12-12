using BCCP.Shared.Constants;
using BCCP.Shared.Extensions;
using BCCP.Shared.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Application.Contracts.Services;
using SecondApplication.Application.Contracts.Services.Grpc;
using SecondApplication.Infrastructure.Contexts;
using SecondApplication.Infrastructure.Mapping;
using SecondApplication.Infrastructure.Repositories;
using SecondApplication.Infrastructure.Services;
using SecondApplication.Infrastructure.Services.Grpc;
using static BCCP.DummyGrpc.Posts;

namespace SecondApplication.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddDatabase(Configuration);
        Services.AddHttpClients(Configuration);
        Services.AddRepositories();
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
        Services.AddHttpClient<IPostService, PostService>();
        Services.AddScoped<IPostService, PostService>();

        BaseServiceConstants.DummyApiBase = Configuration["ApiUrls:DummyApiUrl"];

        return Services;
    }
    private static IServiceCollection AddRepositories(this IServiceCollection Services)
    {
        Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        Services.AddScoped<IPostInfoRepository, PostInfoRepository>();

        return Services;
    }
    private static IServiceCollection AddGrpc(this IServiceCollection Services, IConfiguration Configuration)
    {
        Services.AddGrpcClient<PostsClient>(o =>
        {
            o.Address = new Uri(Configuration["ApiUrls:DummyGrpcUrl"]);
        });

        Services.AddScoped<IPostGrpcService, PostGrpcService>();

        return Services;
    }
}