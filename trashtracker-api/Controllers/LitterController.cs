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
        
    }
}
