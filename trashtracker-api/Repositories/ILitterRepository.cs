using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public interface ILitterRepository
    {
        public Task CreateLitterAsync(Litter litter);
        public Task GetByLitterIdAsync(Guid LitterId);
        public Task GetAllLitterAsync();
        public Task UpdateLitterAsync(Litter litter);
        public Task DeleteLitterAsync(Guid LitterId);
    }
}
