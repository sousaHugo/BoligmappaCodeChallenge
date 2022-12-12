using BCCP.Shared.Constants;
using BCCP.Shared.Requests;
using BCCP.Shared.Responses;
using Newtonsoft.Json;
using System.Text;

namespace BCCP.Shared.Services;

public class BaseService : IBaseService
{
    public IHttpClientFactory _httpClient { get; set; }
    public BaseService(IHttpClientFactory HttpClient)
    {
        _httpClient = HttpClient;
    }

    ///<see cref="IBaseService.SendAsync{T}(ApiRequest)"/>
    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = _httpClient.CreateClient("BoligmappaApi");

            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();
            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                    Encoding.UTF8, "application/json");
            }

            HttpResponseMessage apiResponse = null;
            switch (apiRequest.ApiType)
            {
                case BaseServiceConstants.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case BaseServiceConstants.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case BaseServiceConstants.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
            apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponseDto;

        }
        catch (Exception e)
        {
            var response = new ApiResponse
            {
                DisplayMessage = "Error",
                ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                IsSuccess = false
            };
            var res = JsonConvert.SerializeObject(response);
            throw new Exception(res);
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }
}
