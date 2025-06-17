using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using trashtracker_api.Configurations;
using trashtracker_api.Models;

namespace trashtracker_api.Data
{
    public class LitterDBContext(DbContextOptions<LitterDBContext> options) : DbContext(options)
    {
        public DbSet<Litter> Litters { get; set; }
        public DbSet<WeatherInfo> WeatherInfo { get; set; }
        public DbSet<FavoriteLocation> FavoriteLocations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new LitterConfiguration());
            builder.ApplyConfiguration(new WeatherInfoConfiguration());
            builder.Entity<Litter>()
            .HasOne(l => l.WeatherInfo)
            .WithOne(w => w.Litter)
            .HasForeignKey<Litter>(l => l.Id);
            builder.Entity<Litter>()
            .Navigation(l => l.WeatherInfo)
            .IsRequired();
        }
    }
}
