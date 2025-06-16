using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LitterFromSensoringController : ControllerBase
    {
        private ILitterRepository _litterRepository;

        public LitterFromSensoringController(ILitterRepository litterRepository)
        {
            _litterRepository = litterRepository;

        }

        public async Task<ActionResult<List<Litter>>> PerformAPICall(int id)
        {
            var client = new HttpClient();
            string url;
            if (id == 0)
            {
                url = $"https://avansict2221075.azurewebsites.net/litter/today";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var litter = await response.Content.ReadFromJsonAsync<List<Litter>>();
                    return litter;
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return StatusCode((int)response.StatusCode, "API call failed");
                }
            }
            else if (id == 1)
            {
                url = $"https://avansict2221075.azurewebsites.net/litter";
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var litter = await response.Content.ReadFromJsonAsync<List<Litter>>();
                    return litter;
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                    return StatusCode((int)response.StatusCode, "API call failed");
                }
            }
            else
            {
                Console.WriteLine("No Valid URL");
                return NotFound();
            }

            
        }
        [HttpGet("GetAllLitterFromSensoring")]
        public async Task<ActionResult<List<Litter>>> GetAllLitterDataFromSensoring()
        {
            var litter = await PerformAPICall(1);
            if (litter != null)
            {
                return NotFound();
            }
            return Ok(litter);
        }
        [HttpGet("GetLitterFromSensoring")]
        public async Task<ActionResult<List<Litter>>> GetLitterDataFromSensoring(string beginDate, string EndDate)
        {
            var litter = await PerformAPICall(1);
            if (litter != null)
            {
                return NotFound();
            }
            return Ok(litter);
        }
    }
}
