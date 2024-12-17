using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Spatialdataplatform.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;
using Location = Spatialdataplatform.Models.Location;

namespace Spatialdataplatform.Data
{
    public class SpatialDbContext : DbContext
    {
        public SpatialDbContext(DbContextOptions<SpatialDbContext> options)
            : base(options)
        {
        }

        public DbSet<PointModel>? Points { get; set; }
        public DbSet<PolygonModel>? Polygons { get; set; }
        public DbSet<Location> Locations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure spatial column type
            modelBuilder.Entity<Location>()
                .Property(l => l.Coordinate)
                .HasColumnType("geography(Point, 4326)");
        }
    }
}
