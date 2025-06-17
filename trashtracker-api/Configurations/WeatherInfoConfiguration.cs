using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using trashtracker_api.Models;

namespace trashtracker_api.Configurations
{
    public class WeatherInfoConfiguration : IEntityTypeConfiguration<WeatherInfo>
    {
        public void Configure(EntityTypeBuilder<WeatherInfo> builder)
        {
            builder.ToTable("Litter");
            builder.HasData(
                new WeatherInfo
                {
                    Id = "6B890FF2-FFF0-4A58-8F3E-19797CCC6858",
                    TemperatureCelsius = 20.0f,
                    Humidity = 50.0f,
                    Conditions = "Clear"
                }
            );
        }
    }
}
