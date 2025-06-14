namespace trashtracker_api.Models
{
    public class Litter
    {
        Guid Id;
        int Classification;
        float Confidence;
        float LocationLongitude;
        float LocationLatitude;
        DateTime DetectionTime;
        WeatherInfo WeatherInfo;
    }
}
