using authentication.DbConnections;
using authentication.DTOs;
using authentication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public static User user = new User();

        public UserService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _config = configuration;
        }

        public async Task<User> CreateUserAsync(UserInputDTO userDTO)
        {
            string passwordHash
                = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            user.username = userDTO.Username;
            user.password = passwordHash;
            user.email = userDTO.Email;

            _db.SaveChanges();

            return user;
        }

        public async Task<string> LoginAsync(LoginInputDTO loginDTO)
        {
            if (loginDTO.Email is null) return "Email is required!";

            if (loginDTO == null)
                return "Login credential required!";

            var getUser = await _db.Users.FindAsync(loginDTO.Email);
            if (getUser is null)
                return "User not found";

            bool verifyPasswords = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.password);
            if (!verifyPasswords)
                return "Invalid email/password";

            //string token = GenerateToken(userSession);

            return "Login Success";
        }

        //private string GenerateToken(UserSession user)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var userClaims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(ClaimTypes.Name, user.Name),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };
        //    var token = new JwtSecurityToken(
        //        issuer: _config["Jwt:Issuer"],
        //        audience: _config["Jwt:Audience"],
        //        claims: userClaims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: credentials
        //        );
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
