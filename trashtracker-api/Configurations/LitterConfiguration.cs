using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using trashtracker_api.Models;

namespace trashtracker_api.Configurations
{
    public class LitterConfiguration : IEntityTypeConfiguration<Litter>
    {
        public void Configure(EntityTypeBuilder<Litter> builder)
        {
            builder.ToTable("Litters");

            builder.HasData(
                new Litter
                {
                    Id = "87EC8C26-0E4F-403A-94A5-9B1AC13710AE",
                    Classification = 1,
                    Confidence = 0.95f,
                    LocationLongitude = -122.4194f,
                    LocationLatitude = 37.7749f,
                    DetectionTime = new DateTime(2025, 6, 17, 14, 23, 45, 123)
                }
            );
        }
    }
}
