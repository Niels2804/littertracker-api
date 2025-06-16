using Dapper;
using System.Data;
using trashtracker_api.Models;
using trashtracker_api.Models.Dto;

namespace trashtracker_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task CreateUserAsync(User user)
        {
            var sql = @"
            INSERT INTO [dbo].[Users] (ID, IdentityUserId, DisplayName, ProfilePhotoPath)
            VALUES (@ID, @IdentityUserId, @DisplayName, @ProfilePhotoPath)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public Task DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByUserIDAsync(Guid identityUserId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
