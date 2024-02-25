using authentication.DTOs;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly IAuthManageService _authManageService;

        public AuthManagementController(IAuthManageService authManageService)
        {
            _authManageService = authManageService;
        }

        // USER REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationReqDTO userRegistrationReqDTO)
        {
            var response = await _authManageService.RegisterAsync(userRegistrationReqDTO);

            if (!response.isSucceed) return BadRequest(response); 

            return Ok(response);
        }


        // USER LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqDTO loginReqDTO)
        {
            var loginResult = await _authManageService.LoginAsync(loginReqDTO);

            if (!loginResult.isSucceed) return Unauthorized(loginResult); 

            return Ok(loginResult);
        }


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

            if (!operationResult.isSucceed) return BadRequest(operationResult); 

            return Ok(operationResult);
        }
    }
}
