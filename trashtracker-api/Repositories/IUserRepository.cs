using trashtracker_api.Models;

namespace trashtracker_api.Repositories
{
    public interface IUserRepository
    {
        public Task CreateUserAsync(User user);
        public Task<User> GetUserByUserIDAsync(Guid identityUserId);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(Guid userId);
    }
}
