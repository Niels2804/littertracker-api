using System.Data;
using Dapper;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace trashtracker_api.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly IDbConnection _dbConnection;
        public HolidayRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Holiday> CreateHolidayAsync(Holiday holiday)
        {
            var sql = @"
                INSERT INTO Holidays (Date, LocalName) 
                VALUES (@Date, @LocalName)";
            await _dbConnection.ExecuteAsync(sql, holiday);
            return holiday;
        }

        public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
        {
            var sql = @"
                SELECT Date, LocalName 
                FROM Holidays";
            var holidays = await _dbConnection.QueryAsync<Holiday>(sql);
            return holidays;
        }

        public async Task<Holiday> GetHolidayByDateAsync(DateTime date)
        {
            var sql = @"
                SELECT Date, LocalName 
                FROM Holidays 
                WHERE CAST(Date AS DATE) = @Date";

            var holiday = await _dbConnection.QuerySingleOrDefaultAsync<Holiday>(sql, new { date.Date });
            return holiday;
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysByYear(int year)
        {
            var sql = @"
                SELECT Date, LocalName 
                FROM Holidays 
                WHERE YEAR(Date) = @Year";
            var holidays = await _dbConnection.QueryAsync<Holiday>(sql, new { Year = year });
            return holidays;
        }
    }
}
