using authentication.DbConnections;
using authentication.DTOs;
using authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        // Create user
        public async Task<List<User>> CreateUser(UserInputDTO userDTO)
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

        // Get single user by Id
        public async Task<User?> GetSingleUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null)
                return null;

            return user;
        }

        // Update user by Id
        public async Task<List<User>?> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            var existingUser = await _db.Users.FindAsync(id);
            if (existingUser is null)
                return null;

            existingUser.Username = userUpdateDTO.Username == string.Empty ? existingUser.Username : userUpdateDTO.Username;
            existingUser.Email = userUpdateDTO.Email == string.Empty ? existingUser.Email : userUpdateDTO.Email;

            await _db.SaveChangesAsync();

            return await _db.Users.ToListAsync();
        }

        // Delete user by Id
        public async Task<User?> DeleteUserById(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null)
                return null;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return user;
        }

        // Get all users
        public async Task<List<User>?> GetAllUsers()
        {
            var users = await _db.Users.ToListAsync();
            return users;
        }

        // User login
        public async Task<string> Login(LoginInputDTO loginDTO)
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

            string token = GenerateToken(getUser);

            Console.WriteLine("###########: ", token);

            return token;
        }

        // Generate access token
        private string GenerateToken(User user)
        {
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(
            //        new Claim[] {
            //            new Claim(ClaimTypes.Name, user.Username)
            //        }
            //    ),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
