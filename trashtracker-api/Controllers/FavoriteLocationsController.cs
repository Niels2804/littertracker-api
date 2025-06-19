using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories.Interfaces;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteLocationsController : ControllerBase
    {
        private IFavoriteLocationsRepository _favoriteLocationsRepository;

        public FavoriteLocationsController(IFavoriteLocationsRepository favoriteLocationsRepository)
        {
            _favoriteLocationsRepository = favoriteLocationsRepository;
        }

        // POST / CREATE

        [HttpPost(Name = "CreateFavoriteLocation")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CreateFavoriteLocation(FavoriteLocation favoriteLocation)
        {
            // Validate the favorite location data
            if (favoriteLocation == null ||
                favoriteLocation.UserId == string.Empty ||
                favoriteLocation.LitterId == string.Empty ||
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
            Guid Id = Guid.NewGuid();
            favoriteLocation.Id = Id.ToString();
            var created = await _favoriteLocationsRepository.CreateFavoriteLocationAsync(favoriteLocation);

            if (created == null)
            {
                return BadRequest("Failed to create a new favorite location");
            }

            return CreatedAtRoute(nameof(GetFavoriteLocationsByUserId), new { identityUserId = created.UserId }, created);
        }

        // GET / READ

        [HttpGet("{identityUserId}", Name = "GetFavoriteLocationsByUserId")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<FavoriteLocation>>> GetFavoriteLocationsByUserId([FromRoute] string identityUserId)
        {
            if (identityUserId == string.Empty)
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

        [HttpPut("{favoriteLocationId}", Name = "UpdateFavoriteLocationsById")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateFavoriteLocationsById([FromRoute] string favoriteLocationId, [FromBody] FavoriteLocation updatedFavoriteLocation)
        {
            if (favoriteLocationId == string.Empty ||
                updatedFavoriteLocation == null ||
                updatedFavoriteLocation.Id == string.Empty ||
                updatedFavoriteLocation.UserId == string.Empty ||
                updatedFavoriteLocation.LitterId == string.Empty ||
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

        [HttpDelete("{userId}", Name = "DeleteAllFavoriteLocation")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllFavoriteLocation(string userId)
        {
            var existingFavoriteLocations = await _favoriteLocationsRepository.GetAllFavoriteLocationsAsync(userId);
            if (existingFavoriteLocations == null || !existingFavoriteLocations.Any())
            {
                return NotFound("No favorite locations found for the user");
            }
            await _favoriteLocationsRepository.DeleteAllFavoriteLocationsAsync(userId);
            return NoContent();
        }
    }
}
