using System.Data;
using Dapper;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace trashtracker_api.Repositories
{
    public class LitterRepository : ILitterRepository
    {
        private readonly IDbConnection _dbConnection;
        public LitterRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Litter> CreateLitterAsync(Litter litter)
        {
            var sql = @"
                    INSERT INTO [dbo].[Litter] (Id, Classification, Confidence, LocationLatitude, LocationLongitude, DetectionTime)
                    VALUES (@Id, @Classification, @Confidence, @LocationLatitude, @LocationLongitude, @DetectionTime)";
            await _dbConnection.ExecuteAsync(sql, litter);
            return litter;
        }

        public async Task DeleteLitterAsync(string LitterId)
        {
            var sql = @"
                    DELETE FROM [dbo].[Litter]
                    WHERE Id = @LitterId";
            await _dbConnection.ExecuteAsync(sql, new { LitterId });
        }

        public async Task<IEnumerable<Litter>> GetAllLitterAsync()
        {
            var sql = @"
                    SELECT Id, Classification, Confidence, LocationLatitude, LocationLongitude, DetectionTime
                    FROM [dbo].[Litter]";
            return await _dbConnection.QueryAsync<Litter>(sql);
        }

        public async Task<Litter?> GetByLitterIdAsync(string LitterId)
        {
            var sql = @"
                    SELECT Id, Classification, Confidence, LocationLatitude, LocationLongitude, DetectionTime
                    FROM [dbo].[Litter]
                    WHERE Id = @LitterId";
            return await _dbConnection.QuerySingleOrDefaultAsync<Litter>(sql, new { LitterId });
        }

        public async Task UpdateLitterAsync(Litter litter)
        {
            var sql = @"
                    UPDATE [dbo].[Litter]
                    SET Classification = @Classification, Confidence = @Confidence, LocationLatitude = @LocationLatitude, LocationLongitude = @LocationLongitude, DetectionTime = @DetectionTime
                    WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, litter);
        }
    }
}
