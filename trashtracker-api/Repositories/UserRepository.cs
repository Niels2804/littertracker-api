using Dapper;
using System.Data;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace trashtracker_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var sql = @"
                    INSERT INTO [dbo].[Users] (Id, IdentityUserId, Email, Password, Username, FirstName, LastName, Role)
                    VALUES (@Id, @IdentityUserId, @Email, @Password, @Username, @FirstName, @LastName, @Role)";
            await _dbConnection.ExecuteAsync(sql, user);
            return user;
        }

        public async Task<User> GetUserByIdAsync(string identityUserId)
        {
            var sql = @"
                    SELECT Id, IdentityUserId, Email, Password, Username, FirstName, LastName, Role
                    FROM [dbo].[Users] 
                    WHERE IdentityUserId = @IdentityUserId";
            var user = await _dbConnection.QuerySingleOrDefaultAsync<User>(sql, new { IdentityUserId = identityUserId });
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

        // AUTHENTICATION

        public async Task<string?> GetAuthenticationIdByEmailAsync(string email)
        {
            var sql = @"
                    SELECT Id AS AuthenticationId 
                    FROM [auth].[AspNetUsers] 
                    WHERE Email = @Email";
            return await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new { Email = email });
        }

        public async Task DeleteUserAsync(string authenticationId, string userId)
        {
            var sqlFavoriteLocations = @"
                    DELETE FROM [dbo].[FavoriteLocations] 
                    WHERE UserId = @UserId";
            await _dbConnection.ExecuteAsync(sqlFavoriteLocations, new { UserId = userId });

            var sqlUser = @"
                    DELETE FROM [dbo].[Users] 
                    WHERE IdentityUserId = @AuthenticationId";
            await _dbConnection.ExecuteAsync(sqlUser, new { AuthenticationId = authenticationId });

            var sqlAuthUser = @"
                    DELETE FROM [auth].[AspNetUsers] 
                    WHERE Id = @AuthenticationId";
            await _dbConnection.ExecuteAsync(sqlAuthUser, new { AuthenticationId = authenticationId });
        }
    }
}
