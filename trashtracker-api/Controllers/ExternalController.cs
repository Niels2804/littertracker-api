using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using trashtracker_api.Models;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : ControllerBase
    {
        // GET

        // Getting prediction data
        [HttpGet(Name = "GetPredictionforDate")]
        public async Task<ActionResult<Prediction>> GetPredictionForDate([FromQuery] DateOnly StartDate, [FromQuery] DateOnly EndDate)
        {
            // TO DO: Schrijf iets om predictions te maken
            return null;
        }

        //Getting Holdiay Data

        [HttpGet(Name = "GetHolidays")]
        public async Task GetHolidays([FromBody] int year) // Task<ActionResult<HolidayList>>
        {
            var client = new HttpClient();
            
            string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/NL";
            client.BaseAddress = new Uri(url);

            var json = JsonSerializer.Serialize(new { year });
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = client.PostAsync("posts", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;   
                Console.WriteLine(responseContent);
            } else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }
    }
}
