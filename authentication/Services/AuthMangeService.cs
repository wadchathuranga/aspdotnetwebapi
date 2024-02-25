using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Services
{
    public class AuthMangeService : IAuthManageService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthMangeService(
            IConfiguration configuration,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // User Register
        public async Task<RegisterRes> RegisterAsync(UserRegistrationReqDTO userRegistrationReqDTO)
        {
            var response = new RegisterRes();

            // check email exists
            var emailExists = await _userManager.FindByEmailAsync(userRegistrationReqDTO.Email);

            // check user exists
            if (emailExists != null)
            {
                response.Error = "Email Already Exists!";
                response.isSucceed = false;
                return response;
            }

            // set user info
            var newUser = new AppUser()
            {
                FirstName = userRegistrationReqDTO.FirstName,
                LastName = userRegistrationReqDTO.LastName,
                UserName = userRegistrationReqDTO.Username,
                Email = userRegistrationReqDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            // create user into db
            var isCreated = await _userManager.CreateAsync(newUser, userRegistrationReqDTO.Password);

            if (!isCreated.Succeeded)
            {
                // set response to return
                response.Error = "Something went wrong, User Not Created!"; // isCreated.Errors.Select(e => e.Description).ToList();
                response.isSucceed = false;
                return response;
            }

            // add default user role as USER
            await _userManager.AddToRoleAsync(newUser, UserRoles.USER);

            // generate jwt token
            string token = await GenerateJsonWebTokenAsync(newUser);

            response.Token = token;
            response.isSucceed = true;
            return response;
        }


        // User Login
        public async Task<LoginRes> LoginAsync(UserLoginReqDTO userLoginReqDTO)
        {
            var response = new LoginRes();

            // find the user is existing or not
            var existingUser = await _userManager.FindByEmailAsync(userLoginReqDTO.Email);

            if (existingUser == null)
            {
                response.Error = "User Not Found!";
                response.isSucceed = false;
                return response;
            }

            // check password is valid or not
            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, userLoginReqDTO.Password);

            if (!isPasswordValid)
            {
                response.Error = "Email or Password Invalid!";
                response.isSucceed = false;
                return response;
            }

            // generate jwt token
            string token = await GenerateJsonWebTokenAsync(existingUser);

            response.Token = token;
            response.isSucceed = true;
            return response;
        }


        // ADMIN access is given to the User
        public async Task<CommonResponseHandler> MakeAdminAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var response= new CommonResponseHandler();

            var user = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Email);

            if (user == null) 
            {
                response.Message = "User Not Found!";
                response.isSucceed = false;
                return response;
            }

            var isRoleUpdated = await _userManager.AddToRoleAsync(user!, UserRoles.ADMIN);

            if (isRoleUpdated.Succeeded)
            {
                response.Message = "User has now ADMIN access";
                response.isSucceed = true;
                return response;
            }

            response.Message = "Something went wrong, User role updating failed!";
            response.isSucceed = false;
            return response;
        }


        // OWNER access is given to the User
        public async Task<CommonResponseHandler> MakeOwnerAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var response = new CommonResponseHandler();

            var isUserExists = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Email);

            if (isUserExists == null)
            {
                response.Message = "User Not Found!";
                response.isSucceed = false;
                return response;
            }

            var isRoleUpdated = await _userManager.AddToRoleAsync(isUserExists!, UserRoles.OWNER);

            if (isRoleUpdated.Succeeded)
            {
                response.Message = "User has now OWNER access";
                response.isSucceed = true;
                return response;
            }

            response.Message = "Something went wrong, User role updating failed!";
            response.isSucceed = false;
            return response;
        }


        // Roles seeds to db
        public async Task<CommonResponseHandler> SeedRolesAsync()
        {
            var response = new CommonResponseHandler();

            var isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);
            var isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            var isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
            {
                response.Message = "Roles Seeding Already Done.";
                response.isSucceed = false;
                return response;
            }

            await _roleManager.CreateAsync(new IdentityRole(UserRoles.OWNER));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.USER));

            response.Message = "Roles Seeding Already Done.";
            response.isSucceed = true;
            return response;
        }


        // Generate json web token
        private async Task<string> GenerateJsonWebTokenAsync(AppUser existingUser)
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

        // Generate jwt token
        //private string GenerateJwtToken(AppUser user)
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
    }
}
