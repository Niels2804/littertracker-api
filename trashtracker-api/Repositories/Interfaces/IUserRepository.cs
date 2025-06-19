using trashtracker_api.Models;

namespace trashtracker_api.Repositories.interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(User user);
        public Task<User> GetUserAsync(string username);
        public Task<User> GetUserByIdAsync(string identityUserId);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(string authenticationId, string userId);
        public Task<string?> GetAuthenticationIdByEmailAsync(string email); // Needed for authorization

    }
}
