namespace FirstApplication.Application.Contracts.Services;

public interface IUserInfoService
{
    Task<T> GetAllUserInfoAsync<T>(string Tag);
}
