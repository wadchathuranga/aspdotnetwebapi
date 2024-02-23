using authentication.DTOs;
using authentication.Models;

namespace authentication.Services
{
    public interface IUserService
    {
        Task<List<User>> CreateUser(UserInputDTO userDTO);

        Task<string> Login(LoginInputDTO loginDTO);

        Task<List<User>?> UpdateUser(int id, UserUpdateDTO userUpdateDTO);

        Task<User?> GetSingleUserById(int id);

        Task<User?> DeleteUserById(int id);

        Task<List<User>?> GetAllUsers();
    }
}
