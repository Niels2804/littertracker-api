namespace trashtracker_api.Models
{
    public class User
    {
        public Guid Id;
        public Guid IdentityUserId;
        public string Email;
        public string Password;
        public string Username;
        public string FirstName;
        public string LastName;
        public int Role;
    }
}
