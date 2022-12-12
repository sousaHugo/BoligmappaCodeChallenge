using BCCP.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using SecondApplication.Application.Contracts.Repositories;
using SecondApplication.Domain.Entities;
using SecondApplication.Infrastructure.Contexts;

namespace SecondApplication.Infrastructure.Repositories;

internal class PostInfoRepository : RepositoryBase<PostInfo>, IPostInfoRepository
{
    public PostInfoRepository(ApplicationDbContext DbContext) : base(DbContext)
    {
    }

    public async Task<PostInfo> GetPostInfoByPostIdAsync(string PostId)
    {
        var posts = await GetAsync(a => a.PostId == PostId);

        return posts.FirstOrDefault();
    }
}
