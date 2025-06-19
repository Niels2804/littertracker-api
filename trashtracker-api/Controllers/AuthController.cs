using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordHasher;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace smarth_health.WebApi.Controllers
{
    [ApiController]
    [Route("custom/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserRepository _userRepository;

        public AuthController(UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required");
            }

            // Check if the user already exists
            var existingUser = await _userRepository.GetUserAsync(user.Username);
            if (existingUser != null)
            {
                return BadRequest("User is already registered");
            }

            // Creates the identityUser
            var identityUser = new IdentityUser
            {
                UserName = user.Email,
                Email = user.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Creating a non-identityUser

            // User setup
            user.Id = Guid.NewGuid().ToString(); // Generate a new ID for the user
            user.Password = PasswordHelper.HashPassword(user.Password); // Hashing password and creating user
            user.IdentityUserId = identityUser.Id.ToString(); // Getting Identity User ID

            var createdUser = await _userRepository.CreateUserAsync(user);
            if (createdUser == null)
            {
                return BadRequest("Failed to create (non-identity user)");
            }

            return Ok(user.Id);
        }
    }
}