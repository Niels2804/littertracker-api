using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LitterFromSensoringController : ControllerBase
    {
        // GET / READ

        [HttpGet("GetAllLitterFromSensoring")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Litter>>> GetAllLitterDataFromSensoring()
        {
            var litter = await PerformAPICall(true);
            if (litter != null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        [HttpGet("GetLitterFromSensoring")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Litter>>> GetLitterDataFromSensoring(string beginDate, string EndDate)
        {
            var litter = await PerformAPICall(false);
            if (litter != null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        // FUNCTIONS
        private async Task<ActionResult<List<Litter>>> PerformAPICall(bool getAllData)
        {
            var client = new HttpClient();
            string url;

            url = $"https://avansict2221075.azurewebsites.net/litter/{(getAllData ? "litter" : "today")}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Litter>>();
            }
    
            Console.WriteLine("Error: " + response.StatusCode);
            return StatusCode((int)response.StatusCode, "API call failed");
        }
    }
}
