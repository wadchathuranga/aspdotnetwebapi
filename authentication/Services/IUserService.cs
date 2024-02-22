using authentication.DTOs;
using authentication.Models;
using Microsoft.OpenApi.Any;

namespace authentication.Services
{
    public interface IUserService
    {
        Task<List<User>> CreateUserAsync(UserInputDTO userDTO);

        Task<User> LoginAsync(LoginInputDTO loginDTO);
    }
}
