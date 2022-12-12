using System.Security.AccessControl;
using static BCCP.Shared.Constants.BaseServiceConstants;

namespace BCCP.Shared.Requests;

public class ApiRequest
{
    public ApiType ApiType { get; set; } = ApiType.GET;
    public string Url { get; set; }
    public object Data { get; set; }
}
