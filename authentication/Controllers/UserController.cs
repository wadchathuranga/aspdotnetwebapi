using authentication.DTOs;
using authentication.Models;
using authentication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<List<User>>> Register(UserInputDTO userDTO)
        {
            var response = await _userService.CreateUserAsync(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginInputDTO loginDTO)
        {
            var response = await _userService.LoginAsync(loginDTO);
            return Ok(response);
        }
    }
}
