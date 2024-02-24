using authentication.Configurations;
using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(
            ILogger<WeatherForecastController> logger, 
            UserManager<IdentityUser> userManager, 
            IOptionsMonitor<JwtConfig> _optionMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = _optionMonitor.CurrentValue;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationReqDTO requestDto)
        {
            if (ModelState.IsValid) 
            {
                // check email exists
                var emailExists = await _userManager.FindByEmailAsync(requestDto.Email);

                // check user exists
                if (emailExists != null) return BadRequest("Email Already Exists!");

                // set user info
                var newUser = new IdentityUser 
                { 
                    UserName = requestDto.Username,
                    Email = requestDto.Email 
                };

                // create user into db
                var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);

                if (isCreated.Succeeded) 
                {
                    // generate jwt token
                    var token = GenerateJwtToken(newUser);

                    // set response to return
                    var response = new RegisterRes()
                    {
                        Result = true,
                        Token = token,
                    };

                    return Ok(response);
                }

                return BadRequest(isCreated.Errors.Select(e=>e.Description).ToList());
            }

            return BadRequest("Invalid Request Payload!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqDTO loginReqDTO)
        {
            if (ModelState.IsValid)
            {
                // find the user is existing or not
                var existingUser = await _userManager.FindByEmailAsync(loginReqDTO.Email);
                if (existingUser == null) return BadRequest("Invalid Credentials!");

                // check password is valid or not
                var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, loginReqDTO.Password);
                if (isPasswordValid)
                {
                    // generate jwt token
                    var token = GenerateJwtToken(existingUser);

                    // set response to return
                    var response = new LoginRes()
                    {
                        Result = true,
                        Token = token,
                    };

                    return Ok(response);
                }

                return BadRequest("Invalid Credentials!");
            }

            return BadRequest("Invalid Request Payload!");
        }

        // Generate access token
        private string GenerateJwtToken(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}
