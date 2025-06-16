using trashtracker_api.Models;

namespace trashtracker_api.Repositories
{
    public class LitterRepository : ILitterRepository
    {
        public Task CreateLitterAsync(Litter litter)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLitterAsync(Guid LitterId)
        {
            throw new NotImplementedException();
        }

        public Task<Litter> GetAllLitterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Litter> GetByLitterIdAsync(Guid LitterId)
        {
            throw new NotImplementedException(); 
        }

        public Task UpdateLitterAsync(Litter litter)
        {
            throw new NotImplementedException();
        }
    }
}
