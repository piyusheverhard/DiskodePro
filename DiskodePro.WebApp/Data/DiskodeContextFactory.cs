using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiskodePro.WebApp.Data;

public class DiskodeContextFactory : IDesignTimeDbContextFactory<DiskodeDbContext>
{
    public DiskodeDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<DiskodeDbContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new DiskodeDbContext(optionsBuilder.Options);
    }
}