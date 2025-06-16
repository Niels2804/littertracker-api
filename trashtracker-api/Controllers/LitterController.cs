using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Models;
using trashtracker_api.Repositories;

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
        
        [HttpGet("{identityUserId:guid}", Name = "GetByLitterId")]
        public async Task<ActionResult<Litter>> GetByLitterId([FromBody] Guid LitterId)
        {
            var litter = await _litterRepository.GetByLitterIdAsync(LitterId);
            
            if (litter == null)
                return NotFound();
            
            return Ok(litter);
        }

        [HttpGet("GetAllLitter")]
        public async Task<ActionResult<Litter>> GetAllLitter()
    
        {
            var enviroment = await _litterRepository.GetAllLitterAsync();
            if (enviroment == null)
                return NotFound();

            return Ok(enviroment);
        }
    }
}
