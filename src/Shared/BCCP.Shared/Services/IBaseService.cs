using BCCP.Shared.Requests;
using BCCP.Shared.Responses;

namespace BCCP.Shared.Services;

public interface IBaseService : IDisposable
{
    /// <summary>
    /// This method is responsible for sending HTTP requests depending on the parameter.
    /// </summary>
    /// <param name="ApiRequest">Request is defined by the ApiType (GET, POST, PUT, DELETE), URL and if the request has a body the Data field.</param>
    /// <returns>Generic Type (T) defined when the method is invoked.</returns>
    Task<T> SendAsync<T>(ApiRequest ApiRequest);
}
