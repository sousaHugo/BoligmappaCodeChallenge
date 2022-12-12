using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Services;
using SecondApplication.Application.Contracts.Services;

namespace SecondApplication.Infrastructure.Services;

internal class PostService : BaseService, IPostService
{
    public PostService(IHttpClientFactory HttpClient) : base(HttpClient)
    {
    }
    public async Task<T> GetAllPostsAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Posts"
        });
    }
}
