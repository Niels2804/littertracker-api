namespace trashtracker_api.Models
{
    public class Prediction
    {
        public DateOnly date { get; set; }
        public int predictedTotal { get; set; }
        public float confidence { get; set; }
    }
}
