using FirstApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstApplication.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserInfo> UserInfo { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options) { }
}
