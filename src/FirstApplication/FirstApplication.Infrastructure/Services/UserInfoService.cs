
using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Services;
using FirstApplication.Application.Contracts.Services;

namespace FirstApplication.Infrastructure.Services;

public class UserInfoService : BaseService, IUserInfoService
{
    public UserInfoService(IHttpClientFactory HttpClient) : base(HttpClient)
    {
    }

    public async Task<T> GetAllUserInfoAsync<T>(string Tag)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiAggregatorBase}/UserInfo/{Tag}"
        });
    }
}
