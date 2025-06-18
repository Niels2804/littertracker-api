using trashtracker_api.Models;

namespace trashtracker_api.Repositories.interfaces
{
    public interface ILitterRepository
    {
        public Task<Litter> CreateLitterAsync(Litter litter);
        public Task<Litter?> GetByLitterIdAsync(Guid LitterId);
        public Task<IEnumerable<Litter>> GetAllLitterAsync();
        public Task UpdateLitterAsync(Litter litter);
        public Task DeleteLitterAsync(Guid LitterId);
    }
}
