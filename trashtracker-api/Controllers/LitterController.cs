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

        [HttpGet("{litterId}", Name = "GetByLitterId")]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Litter>> GetByLitterId([FromRoute] string litterId)
        {
            var litter = await _litterRepository.GetByLitterIdAsync(litterId);
            if (litter == null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        [HttpGet("GetAllLitter")]
        [AllowAnonymous] // Must be authorize later
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

        // POST / CREATE

        [HttpPost]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Litter))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Litter>> CreateLitter([FromBody] Litter litter)
        {
            if (litter == null)
            {
                return BadRequest("Litter data is required.");
            }

            var existingLitter = await _litterRepository.GetByLitterIdAsync(litter.Id);
            if (existingLitter != null)
            {
                return BadRequest("Litter with this ID already exists.");
            }

            var createdLitter = await _litterRepository.CreateLitterAsync(litter);
            return CreatedAtRoute("GetByLitterId", new { litterId = createdLitter.Id }, createdLitter);
        }

        // PUT / UPDATE

        [HttpPut("{litterId}")]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Litter))]
        public async Task<ActionResult<Litter>> UpdateLitter([FromRoute] string litterId, [FromBody] Litter litter)
        {
            if (litter == null || litter.Id != litterId)
            {
                return BadRequest("Litter data is required or ID mismatch.");
            }
            var existingLitter = await _litterRepository.GetByLitterIdAsync(litterId);
            if (existingLitter == null)
            {
                return NotFound();
            }
            await _litterRepository.UpdateLitterAsync(litter);
            return Ok(litter);
        }

        // DELETE

        [HttpDelete("{litterId}")]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteLitter([FromRoute] string litterId)
        {
            var existingLitter = await _litterRepository.GetByLitterIdAsync(litterId);
            if (existingLitter == null)
            {
                return NotFound();
            }
            await _litterRepository.DeleteLitterAsync(litterId);
            return NoContent();
        }
    }
}
