using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensoringApi.Models;
using trashtracker_api.Models;
using trashtracker_api.Repositories.interfaces;

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

        // GET / READ

        [HttpGet("GetAllLitterFromSensoring")]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Litter>>> GetAllLitterDataFromSensoring()
        {
            var litter = await PerformAPICall();

            if (litter == null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        [HttpGet("GetLitterFromSensoring")]
        [AllowAnonymous] // Must be authorize later
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Litter>>> GetLitterDataFromSensoring(DateTime beginDate, DateTime endDate)
        {
            if (beginDate > endDate)
            {
                return BadRequest("Begin datum mag niet later zijn dan eind datum.");
            }

            var litter = await PerformAPICall(beginDate, endDate);
            if (litter == null)
            {
                return NotFound();
            }
            return Ok(litter);
        }

        // FUNCTIONS
        private async Task<List<Litter>?> PerformAPICall(DateTime? beginDate = null, DateTime? endDate = null)
        {
            var client = new HttpClient();
            string url;
            List<Litter>? litterList = null;

            if (beginDate.HasValue && endDate.HasValue)
            {
                var beginStr = Uri.EscapeDataString(beginDate.Value.ToString("yyyy-MM-dd"));
                var endStr = Uri.EscapeDataString(endDate.Value.ToString("yyyy-MM-dd"));
                url = $"https://avansict2221075.azurewebsites.net/litter/?beginDate={beginStr}&endDate={endStr}";
            }
            else
            {
                url = "https://avansict2221075.azurewebsites.net/litter";
            }

            try
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadFromJsonAsync<List<LitterResponseDto>>();

                    if (res == null || res.Count == 0)
                    {
                        if (beginDate.HasValue && endDate.HasValue)
                        {
                            var dbData = await _litterRepository.GetAllLitterAsync(beginDate.Value, endDate.Value);
                            litterList = dbData?.ToList();
                        }
                        else
                        {
                            var dbData = await _litterRepository.GetAllLitterAsync();
                            litterList = dbData?.ToList();
                        }
                    }
                    else
                    {
                        litterList = res.Select(l => new Litter
                        {
                            Id = l.litter_id.ToString(),
                            Classification = l.litter_classification,
                            Confidence = (float)l.confidence,
                            LocationLatitude = (float)l.location_latitude,
                            LocationLongitude = (float)l.location_longitude,
                            DetectionTime = l.detection_time ?? DateTime.Now,

                            WeatherInfo = l.weather == null ? null : new WeatherInfo
                            {
                                Id = l.weather.weather_id.ToString(),
                                TemperatureCelsius = (float)l.weather.temperature_celsius,
                                Humidity = (float)l.weather.humidity,
                                Conditions = l.weather.conditions
                            }
                        }).ToList();

                        foreach (var litter in litterList)
                        {
                            var existing = await _litterRepository.GetByLitterIdAsync(litter.Id);
                            if (existing == null)
                            {
                                await _litterRepository.CreateLitterAsync(litter);
                            }
                        }
                    }
                }
                else
                {
                    if (beginDate.HasValue && endDate.HasValue)
                    {
                        var dbData = await _litterRepository.GetAllLitterAsync(beginDate.Value, endDate.Value);
                        litterList = dbData?.ToList();
                    }
                    else
                    {
                        var dbData = await _litterRepository.GetAllLitterAsync();
                        litterList = dbData?.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("API call failed: " + ex.Message);

                if (beginDate.HasValue && endDate.HasValue)
                {
                    var dbData = await _litterRepository.GetAllLitterAsync(beginDate.Value, endDate.Value);
                    litterList = dbData?.ToList();
                }
                else
                {
                    var dbData = await _litterRepository.GetAllLitterAsync();
                    litterList = dbData?.ToList();
                }
            }

            return litterList;
        }
    }
}
