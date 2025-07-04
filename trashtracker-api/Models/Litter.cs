﻿using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class Litter
    {
        [Key]
        public required string Id { get; set; }
        public int Classification { get; set; }
        public float Confidence { get; set; }
        public float LocationLongitude { get; set; }
        public float LocationLatitude { get; set; }
        public DateTime DetectionTime { get; set; }
        public WeatherInfo? WeatherInfo { get; set; }
    }
}
