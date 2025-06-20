using trashtracker_api.Models;

namespace trashtracker_api.Repositories.interfaces
{
    public interface ILitterRepository
    {
        public Task<Litter> CreateLitterAsync(Litter litter);
        public Task<Litter?> GetByLitterIdAsync(string LitterId);
        public Task<IEnumerable<Litter>> GetAllLitterAsync();
        public Task<IEnumerable<Litter>> GetAllLitterAsync(DateTime beginDate, DateTime endDate);
        public Task UpdateLitterAsync(Litter litter);
        public Task DeleteLitterAsync(string LitterId);
    }
}
