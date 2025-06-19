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
            // First insert the Litter (other conflict with FK constraint)
            var sqlLitters = @"
                    INSERT INTO [dbo].[Litters] (Id, Classification, Confidence, LocationLatitude, LocationLongitude, DetectionTime)
                    VALUES (@Id, @Classification, @Confidence, @LocationLatitude, @LocationLongitude, @DetectionTime)";
            await _dbConnection.ExecuteAsync(sqlLitters, litter);

            var sqlWeatherInfo = @"
                    INSERT INTO [dbo].[WeatherInfo] (Id, TemperatureCelsius, Humidity, Conditions)
                    VALUES (@Id, @TemperatureCelsius, @Humidity, @Conditions)";
            await _dbConnection.ExecuteAsync(sqlWeatherInfo, new
            {
                litter.WeatherInfo?.Id,
                litter.WeatherInfo?.TemperatureCelsius,
                litter.WeatherInfo?.Humidity,
                litter.WeatherInfo?.Conditions
            });

            return litter;
        }

        public async Task DeleteLitterAsync(string litterId)
        {
            var sql = @"
                    DELETE FROM [dbo].[Litters]
                    WHERE Id = @LitterId";

            await _dbConnection.ExecuteAsync(sql, new { LitterId = litterId });
        }

        public async Task<IEnumerable<Litter>> GetAllLitterAsync()
        {
            var sql = @"
                    SELECT 
                        l.Id, l.Classification, l.Confidence, l.LocationLatitude, l.LocationLongitude, l.DetectionTime,
                        w.Id, w.TemperatureCelsius, w.Humidity, w.Conditions
                    FROM [dbo].[Litters] l
                    JOIN [dbo].[WeatherInfo] w ON l.Id = w.Id";

            var result = await _dbConnection.QueryAsync<Litter, WeatherInfo, Litter>(
                sql,
                (litter, weather) =>
                {
                    litter.WeatherInfo = weather;
                    return litter;
                },
                splitOn: "Id"
            );

            return result;
        }

        public Task<IEnumerable<Litter>> GetAllLitterAsync(DateTime beginDate, DateTime endDate)
        {
            var sql = @"
                SELECT 
                    l.Id, l.Classification, l.Confidence, l.LocationLatitude, l.LocationLongitude, l.DetectionTime,
                    w.Id, w.TemperatureCelsius, w.Humidity, w.Conditions
                FROM [dbo].[Litters] l
                JOIN [dbo].[WeatherInfo] w ON l.Id = w.Id
                WHERE l.DetectionTime >= @BeginDate AND l.DetectionTime < @EndDate";

            // EndDate wordt opgehoogd met 1 dag → we pakken alles tot 23:59:59.9999999 van de vorige dag
            return _dbConnection.QueryAsync<Litter, WeatherInfo, Litter>(
                sql,
                (litter, weather) =>
                {
                    litter.WeatherInfo = weather;
                    return litter;
                },
                new { BeginDate = beginDate.Date, EndDate = endDate.Date.AddDays(1) },
                splitOn: "Id"
            );
        }


        public async Task<Litter?> GetByLitterIdAsync(string litterId)
        {
            var sql = @"
                    SELECT 
                        l.Id, l.Classification, l.Confidence, l.LocationLatitude, l.LocationLongitude, l.DetectionTime,
                        w.Id, w.TemperatureCelsius, w.Humidity, w.Conditions
                    FROM [dbo].[Litters] l
                    JOIN [dbo].[WeatherInfo] w ON l.Id = w.Id
                    WHERE l.Id = @litterId";

            var result = await _dbConnection.QueryAsync<Litter, WeatherInfo, Litter>(
                sql,
                (litter, weather) =>
                {
                    litter.WeatherInfo = weather;
                    return litter;
                },
                new { litterId },
                splitOn: "TemperatureCelsius" 
            );

            return result.FirstOrDefault();
        }

        public async Task UpdateLitterAsync(Litter litter)
        {
            // First UPDATE the WeatherInfo (other conflict with FK constraint)
            var sqlWeatherInfo = @"
                    UPDATE [dbo].[WeatherInfo] 
                    SET TemperatureCelsius = @TemperatureCelsius, Humidity = @Humidity, Conditions = @Conditions
                    WHERE Id = @Id"; 
            await _dbConnection.ExecuteAsync(sqlWeatherInfo, new
            {
                litter.WeatherInfo?.Id,
                litter.WeatherInfo?.TemperatureCelsius,
                litter.WeatherInfo?.Humidity,
                litter.WeatherInfo?.Conditions
            });

            var sqlLitters = @"
                    UPDATE [dbo].[Litters]
                    SET Classification = @Classification, Confidence = @Confidence, LocationLatitude = @LocationLatitude, LocationLongitude = @LocationLongitude, DetectionTime = @DetectionTime
                    WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sqlLitters, litter);
        }
    }
}
