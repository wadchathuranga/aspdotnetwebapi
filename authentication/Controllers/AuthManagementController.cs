using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
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
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthManagementController(
            ILogger<WeatherForecastController> logger, 
            IConfiguration configuration,
            UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // USER REGISTER
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
                var newUser = new AppUser() 
                { 
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    UserName = requestDto.Username,
                    Email = requestDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                // create user into db
                var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);

                if (isCreated.Succeeded) 
                {
                    // add default user role as USER
                    await _userManager.AddToRoleAsync(newUser, UserRoles.USER);

                    // generate jwt token
                    string token = await GenerateNewJsonWebTokenAsync(newUser);

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


        // USER LOGIN
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
                    // get user roles move to the "GenerateNewJsonWebTokenAsync"
                    //var userRoles = await _userManager.GetRolesAsync(existingUser);

                    // generate jwt token
                    string token = await GenerateNewJsonWebTokenAsync(existingUser);

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
        private async Task<string> GenerateNewJsonWebTokenAsync(AppUser existingUser)
        {
            var tokenSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]!));

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            
            var authClaims = new List<Claim>
                    {
                        new Claim("id", existingUser.Id),
                        new Claim(JwtRegisteredClaimNames.Name, existingUser.UserName!),
                        new Claim(JwtRegisteredClaimNames.Email, existingUser.Email!),
                        new Claim(JwtRegisteredClaimNames.Sub, existingUser.Email!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

            var userRoles = await _userManager.GetRolesAsync(existingUser);

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim("roles", userRole));
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(tokenSecret, SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtWebToken = jwtTokenHandler.WriteToken(token);

            return jwtWebToken;
        }


        // Seeds user roles into db
        [HttpGet("seed-roles")]
        public async Task<IActionResult> SeedUserRoles()
        {
            var isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);
            var isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            var isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists) return Ok("Roles Seeding Already Done."); 

            await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));

            return Ok("User Roles Seeding Done Successfully");
        }


        // Make User as ADMIN
        [HttpPost("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var user = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Username);

            if (user == null) return NotFound("User Not Found!");


            var isRoleUpdated = await _userManager.AddToRoleAsync(user!, UserRoles.ADMIN);

            if (isRoleUpdated.Succeeded) 
            {
                var response = new UserRoleUpdateRes()
                {
                    Result = true,
                    Message = "User has now ADMIN access.",
                };

                return Ok(response);
            }

            return BadRequest("Something went wrong, User role updating fialed!");
        }


        // Make User as OWNER
        [HttpPost("make-owner")]
        public async Task<IActionResult> MakeOwner([FromBody] UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var user = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Username);

            if (user == null) return NotFound("User Not Found!");


            var isRoleUpdated = await _userManager.AddToRoleAsync(user!, UserRoles.OWNER);

            if (isRoleUpdated.Succeeded)
            {
                var response = new UserRoleUpdateRes()
                {
                    Result = true,
                    Message = "User has now OWNER access.",
                };

                return Ok(response);
            }

            return BadRequest("Something went wrong, User role updating fialed!");
        }
    }
}
