using trashtracker_api.Models;

namespace trashtracker_api.Repositories
{
    public interface IFavoriteLocationsRepository
    {
        public Task<FavoriteLocation> GetFavoriteLocationsAsync(Guid favoriteLocationId);
        public Task<FavoriteLocation> GetFavoriteLocationsAsync(Guid userId, Guid litterId);
        public Task<IEnumerable<FavoriteLocation>> GetAllFavoriteLocationsAsync(Guid userId);
        public Task<FavoriteLocation> CreateFavoriteLocationAsync(FavoriteLocation favoriteLocation);
        public Task UpdateFavoriteLocationAsync(Guid favoriteLocationId, FavoriteLocation favoriteLocation);
        public Task DeleteAllFavoriteLocationsAsync(Guid userId);
    }
}
