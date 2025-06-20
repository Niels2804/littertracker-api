using System.ComponentModel.DataAnnotations;

namespace SensoringApi.Models
{
    public class LitterResponseDto
    {
        [Key]
        public Guid litter_id { get; set; }

        [Required]
        public int litter_classification { get; set; }

        [Required]
        public double confidence { get; set; }

        [Required]
        public double location_latitude { get; set; }

        [Required]
        public double location_longitude { get; set; }

        public DateTime? detection_time { get; set; }

        public WeatherResponseDto? weather { get; set; }
    }
}