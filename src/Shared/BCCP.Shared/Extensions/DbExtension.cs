using BCCP.Shared.Constants;
using BCCP.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BCCP.Shared.Extensions;

public static class DbExtension
{
    /// <summary>
    /// This method is responsible for injecting the DbContext as well as configuring the ConnectionString for accessing the PostgreSql database.
    /// </summary>
    public static IServiceCollection AddDatabaseContext<T>(this IServiceCollection Services, IConfiguration Configuration)
        where T : DbContext
    {
        Services.AddDbContext<T>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("UserConnectionString")));

        return Services;
    }

    /// <summary>
    /// This method is responsible for trying to obtain the application's DbContext and, if found, perform the database migration.
    /// </summary>
    public static IServiceCollection MigrateDatabase<T>(this IServiceCollection Services)
        where T : DbContext
    {
        using (ServiceProvider serviceProvider = Services.BuildServiceProvider())
        {
            var dbContext = serviceProvider.GetRequiredService<T>();
            if (dbContext is not null)
                dbContext.Database.Migrate();
        }

        return Services;
    }
}
