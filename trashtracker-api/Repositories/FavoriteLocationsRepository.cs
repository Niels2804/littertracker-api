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

        public async Task<FavoriteLocation> GetFavoriteLocationsAsync(Guid favoriteLocationId)
        {
            var sql = @"SELECT Id, UserId, LitterId, Rating
                    FROM [dbo].[FavoriteLocations]
                    WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<FavoriteLocation>(sql, new { Id = favoriteLocationId });
        }

        public async Task<FavoriteLocation> GetFavoriteLocationsAsync(Guid userId, Guid litterId)
        {
            var sql = @"SELECT Id, UserId, LitterId, Rating
                    FROM [dbo].[FavoriteLocations]
                    WHERE UserId = @UserId AND LitterId = @LitterId";
            return await _dbConnection.QueryFirstOrDefaultAsync<FavoriteLocation>(sql, new { 
                UserId = userId,
                LitterId = litterId
            });
        }

        public async Task<IEnumerable<FavoriteLocation>> GetAllFavoriteLocationsAsync(Guid userId)
        {
            var sql = @"SELECT Id, UserId, LitterId, Rating
                    FROM [dbo].[FavoriteLocations]
                    WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<FavoriteLocation>(sql, new { UserId = userId });
        }

        public async Task<FavoriteLocation> CreateFavoriteLocationAsync(FavoriteLocation favoriteLocation)
        {
            var sql = @"INSERT INTO [dbo].[FavoriteLocations] (Id, UserId, LitterId, Rating) 
                    VALUES (@Id, @UserId, @LitterId, @Rating)";
            await _dbConnection.ExecuteAsync(sql, favoriteLocation);
            return favoriteLocation;
        }

        public async Task UpdateFavoriteLocationAsync(Guid favoriteLocationId, FavoriteLocation favoriteLocation)
        {
            var sql = @"UPDATE [dbo].[FavoriteLocations] 
                    SET UserId = @UserId, LitterId = @LitterId, Rating = @Rating
                    WHERE Id = @FavoriteLocationId";
            await _dbConnection.ExecuteAsync(sql, new 
            { 
                favoriteLocation.UserId,
                favoriteLocation.LitterId,
                favoriteLocation.Rating,
                FavoriteLocationId = favoriteLocation 
            });
        }

        public async Task DeleteAllFavoriteLocationsAsync(Guid userId)
        {
            var sql = @"DELETE FROM [dbo].[FavoriteLocations] 
                    WHERE UserId = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = userId });
        }
    }
}
