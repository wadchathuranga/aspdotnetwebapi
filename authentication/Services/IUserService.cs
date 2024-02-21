using authentication.DTOs;
using authentication.Models;
using Microsoft.OpenApi.Any;

namespace authentication.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(UserInputDTO userDTO);

        Task<string> LoginAsync(LoginInputDTO loginDTO);
    }
}
