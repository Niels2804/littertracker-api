using trashtracker_api.Models;

namespace trashtracker_api.Repositories.Interfaces
{
    public interface IFavoriteLocationsRepository
    {
        public Task<FavoriteLocation?> GetFavoriteLocationsAsync(string favoriteLocationId);
        public Task<FavoriteLocation?> GetFavoriteLocationsAsync(string userId, string litterId);
        public Task<IEnumerable<FavoriteLocation>> GetAllFavoriteLocationsAsync(string userId);
        public Task<FavoriteLocation> CreateFavoriteLocationAsync(FavoriteLocation favoriteLocation);
        public Task UpdateFavoriteLocationAsync(string favoriteLocationId, FavoriteLocation favoriteLocation);
        public Task DeleteAllFavoriteLocationsAsync(string userId);
    }
}
