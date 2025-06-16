using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LitterFromSensoringController
    {
        private ILitterRepository _litterRepository;

        public LitterFromSensoringController(ILitterRepository litterRepository)
        {
            _litterRepository = litterRepository;

        }
    }
}
