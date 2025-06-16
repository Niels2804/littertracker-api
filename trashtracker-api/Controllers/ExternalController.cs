using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using trashtracker_api.Models;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : ControllerBase
    {
        static HttpClient client = new HttpClient();

        // GET

        // Getting prediction data
        [HttpGet(Name = "GetPredictionforDate")]
        public async Task<ActionResult<Prediction>> GetPredictionForDate([FromQuery] DateOnly StartDate, [FromQuery] DateOnly EndDate)
        {
            // TO DO: Schrijf iets om predictions te maken
            return null;
        }

        //Getting Holdiay Data
        [HttpGet(Name = "GetPredictionforDate")]
        public async Task<ActionResult<HolidayList>> GetPredictionForDate([FromBody] int Year)
        {
            string url = $"https://date.nager.at/api/v3/PublicHolidays/{Year}/NL";

            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Fout bij ophalen van feestdagen");
            }

            var holidays = await response.Content.ReadAsAsync<List<Holiday>>();
            return holidays;
        }
    }
}
