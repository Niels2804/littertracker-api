using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using trashtracker_api.Configurations;
using trashtracker_api.Models;

namespace trashtracker_api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext(options)
    {
        public DbSet<Litter> Litters { get; set; }
        public DbSet<WeatherInfo> WeatherInfo { get; set; }
        public DbSet<FavoriteLocation> FavoriteLocations { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(b => b.ToTable("AspNetUsers", "auth"));
            builder.Entity<IdentityRole>(b => b.ToTable("AspNetRoles", "auth"));
            builder.Entity<IdentityUserRole<string>>(b => b.ToTable("AspNetUserRoles", "auth"));
            builder.Entity<IdentityUserClaim<string>>(b => b.ToTable("AspNetUserClaims", "auth"));
            builder.Entity<IdentityUserLogin<string>>(b => b.ToTable("AspNetUserLogins", "auth"));
            builder.Entity<IdentityRoleClaim<string>>(b => b.ToTable("AspNetRoleClaims", "auth"));
            builder.Entity<IdentityUserToken<string>>(b => b.ToTable("AspNetUserTokens", "auth")); 

            builder.ApplyConfiguration(new LitterConfiguration());
            builder.ApplyConfiguration(new WeatherInfoConfiguration());

            // AspNetUsers and User table
            builder.Entity<User>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<User>(u => u.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User and FavoriteLocations table
            builder.Entity<FavoriteLocation>()
                  .HasOne(f => f.User)                         
                  .WithMany(u => u.FavoriteLocations)          
                  .HasForeignKey(f => f.UserId)               
                  .OnDelete(DeleteBehavior.Cascade);

            // Litter and WeatherInfo table
            builder.Entity<Litter>()
                .HasOne(l => l.WeatherInfo)
                .WithOne(w => w.Litter)
                .HasForeignKey<Litter>(l => l.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Litter>()
                .Navigation(l => l.WeatherInfo)
                .IsRequired();
        }
    }
}
