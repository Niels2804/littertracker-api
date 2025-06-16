namespace trashtracker_api.Models
{
    public class FavoriteLocation
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid LitterId { get; set; }
    }
}
