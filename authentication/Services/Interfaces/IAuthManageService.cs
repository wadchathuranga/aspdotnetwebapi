using authentication.DTOs;
using authentication.DTOs.Response;

namespace authentication.Services.Interfaces
{
    public interface IAuthManageService
    {
        Task<CommonResponseHandler> SeedRolesAsync();

        Task<RegisterRes> RegisterAsync(UserRegistrationReqDTO userRegistrationReqDTO);

        Task<LoginRes> LoginAsync(UserLoginReqDTO userLoginReqDTO);

        Task<CommonResponseHandler> MakeAdminAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO);

        Task<CommonResponseHandler> MakeOwnerAsync(UserRoleUpdateReqDTO userRoleUpdateReqDTO);

    }
}
