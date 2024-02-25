using authentication.Controllers;
using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

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

        public Task RegisterAsync(UserRegistrationReqDTO userRegistrationReqDTO)
        {
            throw new NotImplementedException();
        }

        public Task LoginAsync(UserLoginReqDTO userLoginReqDTO)
        {
            throw new NotImplementedException();
        }


        // ADMIN access is given to the User
        public async Task<CommonResponseHandler> MakeAdminAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var roleUpdateRes= new CommonResponseHandler();

            var user = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Username);

            if (user == null) 
            {
                roleUpdateRes.Message = "User Not Found!";
                roleUpdateRes.isSucceed = false;
                return roleUpdateRes;
            }

            var isRoleUpdated = await _userManager.AddToRoleAsync(user!, UserRoles.ADMIN);

            if (isRoleUpdated.Succeeded)
            {
                roleUpdateRes.Message = "User has now ADMIN access";
                roleUpdateRes.isSucceed = true;
                return roleUpdateRes;
            }

            roleUpdateRes.Message = "Something went wrong, User role updating fialed!";
            roleUpdateRes.isSucceed = false;
            return roleUpdateRes;
        }


        // OWNER access is given to the User
        public async Task<CommonResponseHandler> MakeOwnerAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            var response = new CommonResponseHandler();

            var isUserExists = await _userManager.FindByEmailAsync(userRoleUpdateReqDTO.Username);

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

            response.Message = "Something went wrong, User role updating fialed!";
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
    }
}
