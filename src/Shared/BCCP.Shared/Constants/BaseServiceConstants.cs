namespace BCCP.Shared.Constants;

public static class BaseServiceConstants
{
    public static string DummyApiBase { get; set; }
    public static string DummyApiAggregatorBase { get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
