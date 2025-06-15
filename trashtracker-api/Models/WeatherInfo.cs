namespace trashtracker_api.Models
{
    public class WeatherInfo
    {
        public Guid Id { get; set; }
        public float TemperatureCelsius { get; set; }
        public float Humidity { get; set; }
        public string Conditions { get; set; }
    }
}
