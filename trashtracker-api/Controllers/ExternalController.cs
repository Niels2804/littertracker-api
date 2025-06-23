using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

namespace trashtracker_api.Controllers
{
    public class PredictionsResponse
    {
        public IEnumerable<Prediction> Predictions { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ExternalController : ControllerBase
    {
        private IHolidayRepository _holidayRepository;
        public ExternalController(IHolidayRepository holidayRepository)
        {
            _holidayRepository = holidayRepository;
        }

        // GET

        // Getting prediction data
        [HttpGet("predict")]
        [AllowAnonymous] // Must be Authorize later
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Prediction>>> GetPredictionForDate([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date must be earlier than or equal to the end date.");
            }

            var client = new HttpClient();
            string url = $"https://litterapi.jarivankaam.nl/predict/?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var wrapper = await response.Content.ReadFromJsonAsync<PredictionsResponse>();
                return Ok(wrapper.Predictions);
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
            var client = new HttpClient();
            List<Holiday> holidays;

            try
            {
                if (year == 0)
                {
                    year = DateTime.Now.Year;
                }

                string url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/NL";

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadFromJsonAsync<List<Holiday>>();

                    if (res == null || res.Count == 0)
                    {
                        var dbData = await _holidayRepository.GetHolidaysByYear(year);
                        holidays = dbData.ToList();
                    }
                    else {
                        holidays = res;
                        foreach (var holiday in holidays)
                        {
                            var existing = await _holidayRepository.GetHolidayByDateAsync(holiday.Date);
                            if (existing == null)
                            {
                                await _holidayRepository.CreateHolidayAsync(holiday);
                            }
                        }
                    }
                }
                else
                {
                    var dbData = await _holidayRepository.GetHolidaysByYear(year);
                    holidays = dbData.ToList(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("API call failed: " + ex.Message);
                var dbData = await _holidayRepository.GetHolidaysByYear(year);
                holidays = dbData.ToList();
            }

            return holidays;
        }
    }
}
