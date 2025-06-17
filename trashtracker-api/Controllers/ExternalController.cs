using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : ControllerBase
    {
        // GET

        // Getting prediction data
        [HttpGet("predictionForDate")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Prediction>> GetPredictionForDate([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than or equal to the end date.");
            }

            var client = new HttpClient();
            string url = $"https://localhost:7200/{startDate.Year}/NL";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var prediction = await response.Content.ReadFromJsonAsync<Prediction>();
                return Ok(prediction);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return StatusCode((int)response.StatusCode, "API call failed");
            }
        }

        // Getting Holiday Data

        [HttpGet("holidays")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Holiday>))]
        public async Task<ActionResult<List<Holiday>>> GetHolidays([FromQuery] int year = 0)
        {
            if (year == 0)
            {
                year = DateTime.Now.Year;
            }

            var client = new HttpClient();
            string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/NL";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var holidays = await response.Content.ReadFromJsonAsync<List<Holiday>>();
                return Ok(holidays);
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                return StatusCode((int)response.StatusCode, "API call failed");
            }
        }
    }
}
