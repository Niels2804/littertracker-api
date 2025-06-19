using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordHasher;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

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

        // Creating a new user (.../signin)

        [HttpPost("CreateUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(user.IdentityUserId);

            if (existingUser != null)
            {
                return BadRequest("User already exists with this IdentityUserId");
            }

            user.Id = Guid.NewGuid().ToString(); // Generate a new ID for the user
            user.Password = PasswordHelper.HashPassword(user.Password); // Hashing password and creating user

            var createdUser = await _userRepository.CreateUserAsync(user);

            if (createdUser == null)
            {
                return BadRequest("Failed to create user.");
            }

            return CreatedAtRoute("ReadUserByUsername", new { username = user.Username }, createdUser);
        }

        // Verifies or the username and password are valid (.../user/verify)

        [HttpPost("VerifyUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> VerifyUser(User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required");
            }

            // Gets user data from database and verifies the password
            var response = await _userRepository.GetUserAsync(user.Username);

            if (response == null || !PasswordHelper.VerifyHashedPasswordV3(response.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(true);
        }

        // GET / READ

        // API to get all existing users (.../user)

        [HttpGet("id/{identityUserId}", Name = "GetUserById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserById(string identityUserId)
        {
            // Checks or there are any users in the database
            var user = await _userRepository.GetUserByIdAsync(identityUserId);

            if (user == null)
            {
                return NotFound("No users found");
            }

            return Ok(user);
        }

        // API to get an specific user by username (.../user/{username})

        [HttpGet("{username}", Name = "ReadUserByUsername")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            // Checks if the user exists
            var user = await _userRepository.GetUserAsync(username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        // API to get the authentication ID by email (.../user/authentication/id/{email})

        [HttpGet("authentication/id/{email}", Name = "GetAuthenticationIdByEmail")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetAuthenticationIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Emailaddress is required");
            }

            // Gets the authentication ID by email
            var authenticationDto = await _userRepository.GetAuthenticationIdByEmailAsync(email);

            if (authenticationDto == null)
            {
                return NotFound("Email not found");
            }

            return Ok(authenticationDto);
        }

        // UPDATE

        // Updating the user by username (.../user/{username})

        [HttpPut("UpdateUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateUser(User updatedUser)
        {
            if (updatedUser == null)
            {
                return BadRequest("User data is required");
            }

            // Checks or the old / current user exists
            var existing = await _userRepository.GetUserByIdAsync(updatedUser.IdentityUserId);

            if (existing == null)
            {
                return NotFound("User not found");
            }

            // Updating the user old / current user by the old username
            await _userRepository.UpdateUserAsync(updatedUser);

            return Ok(updatedUser);
        }

        // DELETE

        // Delete user by authentication ID (.../user/{authenticationId})

        [HttpDelete("{authenticationId}", Name = "DeleteUserById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(string authenticationId)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(authenticationId);
            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            await _userRepository.DeleteUserAsync(authenticationId, existingUser.Id);
            return NoContent();
        }
    }
}
