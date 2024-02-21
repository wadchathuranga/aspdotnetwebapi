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
        public async Task<IActionResult> Register(UserInputDTO userDTO)
        {
            var response = await _userService.CreateUserAsync(userDTO);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInputDTO loginDTO)
        {
            var response = await _userService.LoginAsync(loginDTO);
            return Ok(response);
        }

        //[HttpPost("register")]
        //public ActionResult<User> Register(UserDto request)
        //{
        //    string passwordHash
        //        = BCrypt.Net.BCrypt.HashPassword(request.Password);

        //    user.Username = request.Username;
        //    user.PasswordHash = passwordHash;

        //    return Ok(user);
        //}

        //[HttpPost("login")]
        //public IActionResult Login(LoginInputDto request)
        //{
        //    if (user.Username != request.Username)
        //    {
        //        return BadRequest("User not found.");
        //    }

        //    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        //    {
        //        return BadRequest("Wrong password.");
        //    }

        //    string token = CreateToken(user);

        //    return Ok(token);
        //}
    }
}
