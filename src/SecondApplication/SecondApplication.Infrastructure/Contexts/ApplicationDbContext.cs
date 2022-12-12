using Microsoft.EntityFrameworkCore;
using SecondApplication.Domain.Entities;

namespace SecondApplication.Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public DbSet<PostInfo> Postnfo { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> Options) : base(Options) { }
}
