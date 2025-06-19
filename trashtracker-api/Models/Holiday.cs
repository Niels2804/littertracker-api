using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class Holiday
    {
        [Key]
        public DateTime Date { get; set; }
        public string LocalName { get; set; }
    }
}
