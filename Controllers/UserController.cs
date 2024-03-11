using Microsoft.AspNetCore.Mvc;
using UserApiWebPic.Domain;
using UserApiWebPic.Serivces;

namespace UserApiWebPic.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await _userService.GetAll();
        }

        [HttpGet("pagination")]
        public async Task<IActionResult> GetAllpagination(int page = 1, int pageSize = 10)
        {
            return await _userService.GetAllpagination(page, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return await _userService.GetById(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            return await _userService.Post(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User user)
        {
            return await _userService.Put(id, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _userService.Delete(id);
        }
    }
}
