using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public class FavoriteLocationsRepository : IFavoriteLocationsRepository
    {
        public Task CreateUserAsync(FavoriteLocation favoriteLocation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(Guid LocationId)
        {
            throw new NotImplementedException();
        }

        public Task GetAllFavoriteLocationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserFavoriteLocationIdAsync(Guid locationId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(FavoriteLocation favoriteLocation)
        {
            throw new NotImplementedException();
        }
    }
}
