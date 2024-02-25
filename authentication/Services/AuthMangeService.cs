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

        public Task LoginAsync(UserLoginReqDTO userLoginReqDTO)
        {
            throw new NotImplementedException();
        }

        public Task MakeAdminAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            throw new NotImplementedException();
        }

        public Task MakeOwnerAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO)
        {
            throw new NotImplementedException();
        }

        public Task RegisterAsync(UserRegistrationReqDTO userRegistrationReqDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponseHandler> SeedRolesAsync()
        {
            var response = new CommonResponseHandler();

            var isOwnerRoleExists = await _roleManager.RoleExistsAsync(UserRoles.OWNER);
            var isAdminRoleExists = await _roleManager.RoleExistsAsync(UserRoles.ADMIN);
            var isUserRoleExists = await _roleManager.RoleExistsAsync(UserRoles.USER);

            if (isOwnerRoleExists && isAdminRoleExists && isUserRoleExists)
            {
                response.Message = "Roles Seeding Already Done.";
                response.isSucceed = true;
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
