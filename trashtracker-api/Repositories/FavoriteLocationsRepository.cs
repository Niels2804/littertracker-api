using Dapper;
using System.Data;
using trashtracker_api.Models;

namespace trashtracker_api.Repositories
{
    public class FavoriteLocationsRepository : IFavoriteLocationsRepository
    {
        private readonly IDbConnection _dbConnection;
        public FavoriteLocationsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task CreateUserAsync(FavoriteLocation favoriteLocation)
        {
            var sql = @"
            INSERT INTO [dbo].[FavoriteLocations] (Id, UserId, LitterId)
            VALUES (@Id, @UserId, @LitterId)";
            await _dbConnection.ExecuteAsync(sql, favoriteLocation);
        }

        public async Task DeleteUserAsync(Guid LocationId)
        {
            var sql = @"
            DELETE FROM [dbo].[FavoriteLocations] WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql);
        }

        public async Task<IEnumerable<FavoriteLocation>> GetAllFavoriteLocationsAsync()
        {
            var sql = @"
            SELECT Id, UserId, LitterId FROM [dbo].[FavoriteLocations]";
            await _dbConnection.ExecuteAsync(sql);
        }

        public async Task<FavoriteLocation> GetUserFavoriteLocationIdAsync(Guid locationId)
        {
            var sql = @"
            SELECT Id, UserId, LitterId FROM [dbo].[FavoriteLocations] WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, locationId);
        }

        public async Task UpdateUserAsync(FavoriteLocation favoriteLocation)
        {
            throw new NotImplementedException();
        }
    }
}
