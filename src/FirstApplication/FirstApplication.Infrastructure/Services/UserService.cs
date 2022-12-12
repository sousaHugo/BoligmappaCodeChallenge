using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Services;
using FirstApplication.Application.Contracts.Services;

namespace FirstApplication.Infrastructure.Services;
public class UserService : BaseService, IUserService
{
    public UserService(IHttpClientFactory HttpClient) : base(HttpClient)
    {
    }

    public async Task<T> GetAllUsersAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = BaseServiceConstants.DummyApiBase + "/Users"
        });
    }
    public async Task<T> GetAllUsersByUsernameAsync<T>(string Username)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Users/ByUsername/{Username}"
        });
    }
}
