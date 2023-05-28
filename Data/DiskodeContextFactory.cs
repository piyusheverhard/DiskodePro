using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Diskode.Data
{
    public class DiskodeContextFactory : IDesignTimeDbContextFactory<DiskodeDbContext>
    {
        public DiskodeDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

            DbContextOptionsBuilder<DiskodeDbContext> optionsBuilder = new DbContextOptionsBuilder<DiskodeDbContext>();
            optionsBuilder.UseMySql(connectionString, serverVersion);

            return new DiskodeDbContext(optionsBuilder.Options);
        }
    }
}
