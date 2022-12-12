namespace FirstApplication.Application.Contracts.Services;

public interface IPostService
{
    Task<T> GetAllPostsAsync<T>();
    Task<T> GetAllPostsByUsernameAsync<T>(string Username);
    Task<T> GetAllPostsByTagAsync<T>(string Tag);
}
