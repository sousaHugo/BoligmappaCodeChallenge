namespace FirstApplication.Application.Contracts.Services;

public interface ITodoService
{
    Task<T> GetAllTodosAsync<T>();
    Task<T> GetAllTodosByUserIdAsync<T>(string UserId);
}
