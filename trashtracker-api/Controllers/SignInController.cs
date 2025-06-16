using Microsoft.AspNetCore.Mvc;
using trashtracker_api.Repositories;

namespace trashtracker_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignInController
    {
        private IUserRepository _userRepository;

        public SignInController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
