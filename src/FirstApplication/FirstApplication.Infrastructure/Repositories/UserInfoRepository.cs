using BCCP.Shared.Repositories;
using FirstApplication.Application.Contracts.Repositories;
using FirstApplication.Domain.Entities;
using FirstApplication.Infrastructure.Contexts;

namespace FirstApplication.Infrastructure.Repositories;

public class UserInfoRepository : RepositoryBase<UserInfo>, IUserInfoRepository
{
    public UserInfoRepository(ApplicationDbContext DbContext) : base(DbContext)
    {
    }

    public async Task<UserInfo> GetByUserIdAsync(string UserId)
    {
        var userEf = await GetAsync(a => a.UserId == UserId);

        return userEf.FirstOrDefault();
    }

    public async Task<IEnumerable<UserInfo>> GetUserMoreTwoPostsAsync()
    {
        return await GetAsync(a => a.NumberOfPosts > 2);
    }
    public async Task<IEnumerable<UserInfo>> GetUserUseMasterCardAsync()
    {
        return await GetAsync(a => a.UseMasterCard);
    }
}
