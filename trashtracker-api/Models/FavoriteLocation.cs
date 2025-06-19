using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class FavoriteLocation
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LitterId { get; set; }
        public int Rating { get; set; }
        public User? User { get; set; }
    }
}
