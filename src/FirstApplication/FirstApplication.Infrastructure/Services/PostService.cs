using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Services;
using FirstApplication.Application.Contracts.Services;

namespace FirstApplication.Infrastructure.Services;
public class PostService : BaseService, IPostService
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

    public async Task<T> GetAllPostsByUsernameAsync<T>(string Username)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Posts/ByUsername/{Username}"
        });
    }
    public async Task<T> GetAllPostsByTagAsync<T>(string Tag)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Posts/ByTag/{Tag}"
        });
    }
}
