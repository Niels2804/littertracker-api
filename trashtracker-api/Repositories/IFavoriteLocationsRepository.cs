using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public interface IFavoriteLocationsRepository
    {
        public Task GetAllFavoriteLocationsAsync();
        public Task<UserDto> GetUserFavoriteLocationIdAsync(Guid locationId);
        public Task CreateUserAsync(FavoriteLocation favoriteLocation);
        public Task UpdateUserAsync(FavoriteLocation favoriteLocation);
        public Task DeleteUserAsync(Guid LocationId);
    }
}
