using System.ComponentModel.DataAnnotations;

namespace trashtracker_api.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Missing IdentityUserID")]
        public Guid IdentityUserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Role { get; set; }
    }
}
