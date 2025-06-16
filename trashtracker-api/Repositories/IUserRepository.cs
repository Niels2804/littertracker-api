using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public interface IUserRepository
    {
        public Task CreateUserAsync(User user);
        public Task<UserDto> GetUserByUserIDAsync(Guid identityUserId);
        public Task UpdateUserAsync(User user);
        public Task DeleteUserAsync(Guid userId);
    }
}
