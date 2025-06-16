using Dapper;
using System.Data;
using trashtracker_api.Models;

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
            INSERT INTO [dbo].[Users] (Id, IdentityUserId, Email, Password, Username, FirstName, LastName, Role)
            VALUES (@ID, @IdentityUserId, @Email, @Password, @Username, @FirstName, @LastName, @Role)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var sql = @$"
            DELETE FROM [dbo].[Users] WHERE IdentityUserId = @IdentityUserId";
            await _dbConnection.ExecuteAsync(sql, new { IdenityUserId = userId});
        }

        public async Task<User> GetUserByUserIDAsync(Guid identityUserId)
        {
            var sql = @"
            SELECT Id, IdentityUserId, Email, Password, Username, FirstName, LastName, Role
            FROM [dbo].[Users] 
            WHERE IdentityUserId = @IdentityUserId";
            var user = await _dbConnection.QuerySingleOrDefault(sql, new {IdentityUserId = identityUserId });
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            var sql = @$"
            UPDATE [dbo].[Users] 
            SET Id = @Id, IdentityUserId = @IdentityUserId, Email = @Email, Password = @Password, Username = @Username, FirstName = @FirstName, LastName = @LastName, Role = @Role
            WHERE IdentityUserId = @IdentityUserId";
            await _dbConnection.ExecuteAsync(sql, user);
        }
    }
}
