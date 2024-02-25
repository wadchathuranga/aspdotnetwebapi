using authentication.DTOs;
using authentication.DTOs.Response;

namespace authentication.Services.Interfaces
{
    public interface IAuthManageService
    {
        Task<CommonResponseHandler> SeedRolesAsync();

        Task RegisterAsync(UserRegistrationReqDTO userRegistrationReqDTO);

        Task LoginAsync(UserLoginReqDTO userLoginReqDTO);

        Task MakeAdminAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO);

        Task MakeOwnerAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO);

    }
}
