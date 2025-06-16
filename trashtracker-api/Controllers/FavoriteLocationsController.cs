using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteLocationsController : ControllerBase
    {
        private IFavoriteLocationsRepository _favoriteLocationsRepository;

        public FavoriteLocationsController (IFavoriteLocationsRepository favoriteLocationsRepository)
        {
            _favoriteLocationsRepository = favoriteLocationsRepository;
        }

        // POST / CREATE

        [HttpPost(Name = "CreateFavoriteLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateFavoriteLocation(FavoriteLocation favoriteLocation)
        {
            // Validate the favorite location data
            if (favoriteLocation == null ||
                favoriteLocation.UserId == Guid.Empty ||
                favoriteLocation.LitterId == Guid.Empty ||
                favoriteLocation.Rating < 0)
            {
                return BadRequest("Invalid favorite location data");
            }

            // Check if the favorite location already exists and increment the rating if it does
            var existing = await _favoriteLocationsRepository.GetFavoriteLocationsAsync(favoriteLocation.UserId, favoriteLocation.LitterId);
            if (existing != null)
            {
                existing.Rating += 1;
                await _favoriteLocationsRepository.UpdateFavoriteLocationAsync(existing.Id, existing);
                return Ok("Favorite location rating increased");
            }

            // Create a new favorite location
            favoriteLocation.Id = Guid.NewGuid();
            var created = await _favoriteLocationsRepository.CreateFavoriteLocationAsync(favoriteLocation);
            
            if (created == null)
            {
                return BadRequest("Failed to create a new favorite location");
            }

            return CreatedAtRoute("GetFavoriteLocationsByUserId", new { identityUserId = created.UserId }, created);
        }

        // GET / READ

        [HttpGet("{identityUserId:guid}", Name = "GetFavoriteLocationsByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FavoriteLocation>>> GetFavoriteLocationsByUserId([FromRoute] Guid identityUserId)
        {
            if (identityUserId == Guid.Empty)
            {
                return BadRequest("User ID is required");
            }

            var favoriteLocations = await _favoriteLocationsRepository.GetAllFavoriteLocationsAsync(identityUserId);

            if (favoriteLocations == null || !favoriteLocations.Any())
            {
                return NotFound("No items found");
            }

            return Ok(favoriteLocations);
        }

        // UPDATE

        [HttpPut("{favoriteLocationId:guid}", Name = "UpdateFavoriteLocationsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateFavoriteLocationsById([FromRoute] Guid favoriteLocationId, [FromBody] FavoriteLocation updatedFavoriteLocation)
        {
            if (favoriteLocationId == Guid.Empty ||
                updatedFavoriteLocation == null ||
                updatedFavoriteLocation.Id == Guid.Empty ||
                updatedFavoriteLocation.UserId == Guid.Empty ||
                updatedFavoriteLocation.LitterId == Guid.Empty ||
                updatedFavoriteLocation.Rating < 0)
            {
                return BadRequest("Invalid favorite location data");
            }

            // Check if the favorite location exists
            var existing = await _favoriteLocationsRepository.GetFavoriteLocationsAsync(favoriteLocationId);
            if (existing == null)
            {
                return NotFound("Favorite location not found");
            }

            await _favoriteLocationsRepository.UpdateFavoriteLocationAsync(favoriteLocationId, updatedFavoriteLocation);

            return Ok();
        }

        // DELETE

        [HttpDelete("{userId:guid}", Name = "DeleteAllFavoriteLocation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllFavoriteLocation(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("user ID is required");
            }

            await _favoriteLocationsRepository.DeleteAllFavoriteLocationsAsync(userId);
            return NoContent();
        }
    }
}
