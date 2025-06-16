using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST

        // Creating a new user
        [HttpPost(Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.Id = Guid.NewGuid();
            await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUserByUserId), new { identityUserId = user.IdentityUserId }, user);
        }

        // Getting a specific user by identityUserId
        [HttpGet("{identityUserId:guid}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByUserId([FromBody] Guid identityUserId)
        {
            var user = await _userRepository.GetUserByUserIDAsync(identityUserId);

            // If user is not found, return 404
            if (user == null)
                return NotFound();

            // Mapping
            var mappedUser = new User()
            {
                Id = user.Id,
                IdentityUserId = Guid.Parse(user.IdentityUserId),
                Email = user.Email,
                Password = user.Password,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role
            };

            return Ok(mappedUser);
        }
        // UPDATE

        // Updating user by userId
        [HttpPut("{userId:guid}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] User user)
        {
            // Check if userId is the same as user.ID
            if (userId != user.Id)
                return BadRequest();
            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        // DELETE

        // Deleting user by userId
        [HttpDelete("{identityUserIderId:guid}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteUser(Guid identityUserIderId)
        {
            await _userRepository.DeleteUserAsync(identityUserIderId);
            return NoContent();
        }
    }
}
