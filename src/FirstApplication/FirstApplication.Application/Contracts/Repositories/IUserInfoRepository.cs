using FirstApplication.Domain.Entities;

namespace FirstApplication.Application.Contracts.Repositories;

public interface IUserInfoRepository
{
    Task<IEnumerable<UserInfo>> GetUserMoreTwoPostsAsync();
    Task<IEnumerable<UserInfo>> GetUserUseMasterCardAsync();
    Task<UserInfo> AddAsync(UserInfo Entity);
    Task<IReadOnlyList<UserInfo>> GetAllAsync();
    Task<UserInfo> GetByUserIdAsync(string UserId);
    Task<bool> Exists(string Id);
    Task UpdateAsync(UserInfo Entity);
}
