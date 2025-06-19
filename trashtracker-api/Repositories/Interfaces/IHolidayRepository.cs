using trashtracker_api.Models;

namespace trashtracker_api.Repositories.interfaces
{
    public interface IHolidayRepository
    {
        public Task<Holiday> CreateHolidayAsync(Holiday holiday);
        public Task<Holiday> GetHolidayByDateAsync(DateTime date);
        public Task<IEnumerable<Holiday>> GetHolidaysByYear(int year);
        public Task<IEnumerable<Holiday>> GetAllHolidaysAsync();
    }
}
