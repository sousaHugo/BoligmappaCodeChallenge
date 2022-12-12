namespace SecondApplication.Application.Contracts.Services;

public interface IPostService
{
    /// <summary>
    /// This method is responsible for getting the Post Information from the dummy api.
    /// </summary>
    /// <returns>T - Type wich must be used to convert the information from the dummy api.</returns>
    Task<T> GetAllPostsAsync<T>();
}
