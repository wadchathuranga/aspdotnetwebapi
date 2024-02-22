using authentication.DbConnections;
using authentication.DTOs;
using authentication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public UserService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _config = configuration;
        }

        public async Task<List<User>> CreateUserAsync(UserInputDTO userDTO)
        {
            
            var user = new User()
            {
                Username = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
            };

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            user.Username = userDTO.Username;
            user.Email = userDTO.Email;
            user.Password = passwordHash;

            _db.Users.Add(user);
            _db.SaveChanges();

            return await _db.Users.ToListAsync();
        }

        public async Task<User> LoginAsync(LoginInputDTO loginDTO)
        {
            if (loginDTO.Email is null) throw new Exception("Email is required!");

            if (loginDTO == null)
                throw new Exception("Login credential required!");

            var getUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (getUser is null)
                throw new Exception("User not found");

            bool verifyPasswords = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
            if (!verifyPasswords)
                throw new Exception("Invalid email/password");

            //string token = GenerateToken(userSession);

            return getUser;
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
