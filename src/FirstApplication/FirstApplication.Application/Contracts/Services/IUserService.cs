namespace FirstApplication.Application.Contracts.Services;

public interface IUserService
{
    Task<T> GetAllUsersAsync<T>();
    Task<T> GetAllUsersByUsernameAsync<T>(string Username);
}
