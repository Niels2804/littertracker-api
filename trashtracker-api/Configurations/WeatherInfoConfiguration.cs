using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using trashtracker_api.Models;

namespace trashtracker_api.Configurations
{
    public class WeatherInfoConfiguration : IEntityTypeConfiguration<WeatherInfo>
    {
        public void Configure(EntityTypeBuilder<WeatherInfo> builder)
        {
            builder.ToTable("WeatherInfo");
            builder.HasData(
                new WeatherInfo
                {
                    Id = "87EC8C26-0E4F-403A-94A5-9B1AC13710AE",
                    TemperatureCelsius = 20.0f,
                    Humidity = 50.0f,
                    Conditions = "Clear"
                }
            );
        }
    }
}
