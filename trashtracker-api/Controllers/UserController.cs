using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories;
using Microsoft.AspNetCore.Mvc.Controllers;


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
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserByUserID(Guid userId)
        {
            var user = await _userRepository.GetUserByUserIDAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpPost(Name = "CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public void CreateUser([FromBody] User user)
        {
            User. = Guid.NewGuid();
            await _userRepository.InsertAsync(user);
            return CreatedAtAction(nameof(GetUser), new { identityUserId = user.IdentityUserID }, user);
        }
    }
}
