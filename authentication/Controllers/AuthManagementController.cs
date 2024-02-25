using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using authentication.Services.Interfaces;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthManageService _authManageService;

        public AuthManagementController(
            IConfiguration configuration,
            IAuthManageService authManageService,
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _authManageService = authManageService;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // USER REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationReqDTO userRegistrationReqDTO)
        {
            var response = await _authManageService.RegisterAsync(userRegistrationReqDTO);

            if (response.isSucceed) return BadRequest(response); 

            return Ok(response);
        }


        // USER LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqDTO loginReqDTO)
        {
            var loginResult = await _authManageService.LoginAsync(loginReqDTO);

            if (loginResult.isSucceed) return Unauthorized(loginResult); 

            return Ok(loginResult);
        }


        // Generate jwt token
        //private string GenerateJwtToken(IdentityUser user)
        //{
        //    var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]!);

        //    var jwtTokenHandler = new JwtSecurityTokenHandler();

        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(new Claim[] 
        //        {
        //            new Claim("Id", user.Id),
        //            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
        //            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
        //    };
        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        //    var jwtToken = jwtTokenHandler.WriteToken(token);
        //    return jwtToken;
        //}


        // Generate new jwt web token
        //private async Task<string> GenerateNewJsonWebTokenAsync(AppUser existingUser)
        //{
        //    var tokenSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]!));

        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
            
        //    var authClaims = new List<Claim>
        //            {
        //                new Claim("id", existingUser.Id),
        //                new Claim(JwtRegisteredClaimNames.Name, existingUser.UserName!),
        //                new Claim(JwtRegisteredClaimNames.Email, existingUser.Email!),
        //                new Claim(JwtRegisteredClaimNames.Sub, existingUser.Email!),
        //                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //            };

        //    var userRoles = await _userManager.GetRolesAsync(existingUser);

        //    foreach (var userRole in userRoles)
        //    {
        //        authClaims.Add(new Claim("roles", userRole));
        //    }

        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(authClaims),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(tokenSecret, SecurityAlgorithms.HmacSha512)
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        //    var jwtWebToken = jwtTokenHandler.WriteToken(token);

        //    return jwtWebToken;
        //}


        // User roles seeds into db
        [HttpGet("seed-roles")]
        public async Task<IActionResult> SeedUserRoles()
        {
            var response = await _authManageService.SeedRolesAsync();

            if (!response.isSucceed) return BadRequest(response); 

            return Ok(response);
        }


        // Make User as ADMIN
        [HttpPost("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var response = await _authManageService.MakeAdminAsync(userRoleUpdateReqDTO);

            if (!response.isSucceed) return BadRequest(response); 

            return Ok(response);
        }


        // Make User as OWNER
        [HttpPost("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var operationResult = await _authManageService.MakeOwnerAsync(userRoleUpdateReqDTO);

            if (operationResult.isSucceed) return BadRequest(operationResult); 

            return Ok(operationResult);
        }
    }
}
