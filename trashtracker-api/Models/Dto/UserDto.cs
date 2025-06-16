namespace trashtracker_api.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? IdentityUserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Role { get; set; }
    }
}
