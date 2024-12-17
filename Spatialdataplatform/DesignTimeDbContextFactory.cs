using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Spatialdataplatform.Data;

namespace Spatialdataplatform
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SpatialDbContext>
    {
        public SpatialDbContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json or environment variables
            var optionsBuilder = new DbContextOptionsBuilder<SpatialDbContext>();

            // Make sure to provide the correct connection string
            optionsBuilder.UseNpgsql(
                "Host=127.0.0.1;Port=5432;Database=SpatialData;Username=postgres;Password=Sainadhkd@123",
                options => options.UseNetTopologySuite()
            );

            return new SpatialDbContext(optionsBuilder.Options);
        }
    }
}
