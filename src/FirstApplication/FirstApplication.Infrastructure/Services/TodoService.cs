using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Services;
using FirstApplication.Application.Contracts.Services;

namespace FirstApplication.Infrastructure.Services;
public class TodoService : BaseService, ITodoService
{
    public TodoService(IHttpClientFactory HttpClient) : base(HttpClient)
    {
    }
    public async Task<T> GetAllTodosAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Todos"
        });
    }

    public async Task<T> GetAllTodosByUserIdAsync<T>(string UserId)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = BaseServiceConstants.ApiType.GET,
            Url = $"{BaseServiceConstants.DummyApiBase}/Todos/{UserId}"
        });
    }
}
