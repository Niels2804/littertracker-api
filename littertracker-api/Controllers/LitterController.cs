using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LitterController : ControllerBase
    {
        private ILitterRepository _litterRepository;

        public LitterController(ILitterRepository litterRepository)
        {
            _litterRepository = litterRepository;
        }

        // GET / READ
        
        [HttpGet("{litterId:guid}", Name = "GetByLitterId")]
        [AllowAnonymous] // Must be [Authorize] later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Litter>> GetByLitterId([FromRoute] Guid litterId)
        {
            var litter = await _litterRepository.GetByLitterIdAsync(litterId);
            if (litter == null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        [HttpGet("GetAllLitter")]
        [AllowAnonymous] // Must be [Authorize] later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Litter>> GetAllLitter()
        {
            var litter = await _litterRepository.GetAllLitterAsync();
            if (litter == null)
            {
                return NotFound();
            }
            return Ok(litter);
        }
    }
}
