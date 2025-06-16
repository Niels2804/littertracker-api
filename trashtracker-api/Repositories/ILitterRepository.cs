using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public interface ILitterRepository
    {
        public Task CreateLitterAsync(Litter litter);
        public Task<Litter> GetByLitterIdAsync(Guid LitterId);
        public Task<Litter> GetAllLitterAsync();
        public Task UpdateLitterAsync(Litter litter);
        public Task DeleteLitterAsync(Guid LitterId);
    }
}
