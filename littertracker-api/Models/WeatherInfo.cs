using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class WeatherInfo
    {
        [Key]
        public required string Id { get; set; }
        public float TemperatureCelsius { get; set; }
        public float Humidity { get; set; }
        [StringLength(100)]
        public string? Conditions { get; set; }
        public Litter? Litter { get; set; }
    }
}
