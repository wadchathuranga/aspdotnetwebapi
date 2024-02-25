using authentication.DbConnections;
using authentication.DTOs;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace authentication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public EmployeeService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _config = configuration;
        }

        // Create user
        public async Task<List<Employee>> CreateEmployee(EmployeeCreateDTO employeeCreateDTO)
        {
            
            var user = new Employee()
            {
                EmpName = employeeCreateDTO.EmpName,
                EmpEmail = employeeCreateDTO.EmpEmail,
                EmpAddress = employeeCreateDTO.EmpAddress,
            };

            _db.Employees.Add(user);
            _db.SaveChanges();

            return await _db.Employees.ToListAsync();
        }

        // Get single user by Id
        public async Task<Employee?> GetSingleEmployeeById(int id)
        {
            var user = await _db.Employees.FindAsync(id);
            if (user is null)
                return null;

            return user;
        }

        // Update user by Id
        public async Task<List<Employee>?> UpdateEmployee(int id, EmployeeUpdateDTO employeeUpdateDTO)
        {
            var existingUser = await _db.Employees.FindAsync(id);
            if (existingUser is null)
                return null;

            existingUser.EmpName = employeeUpdateDTO.EmpName == string.Empty ? existingUser.EmpName : employeeUpdateDTO.EmpName;
            existingUser.EmpEmail = employeeUpdateDTO.EmpEmail == string.Empty ? existingUser.EmpEmail : employeeUpdateDTO.EmpEmail;

            await _db.SaveChangesAsync();

            return await _db.Employees.ToListAsync();
        }

        // Delete user by Id
        public async Task<Employee?> DeleteEmployeeById(int id)
        {
            var user = await _db.Employees.FindAsync(id);
            if (user is null)
                return null;

            _db.Employees.Remove(user);
            await _db.SaveChangesAsync();

            return user;
        }

        // Get all Employees
        public async Task<List<Employee>?> GetAllEmployees()
        {
            var Employees = await _db.Employees.ToListAsync();
            return Employees;
        }

        // User login
        //public async Task<string> Login(LoginDTO loginDTO)
        //{
        //    if (loginDTO.Email is null) throw new Exception("Email is required!");

        //    if (loginDTO == null)
        //        throw new Exception("Login credential required!");

        //    var getUser = await _db.Employees.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

        //    if (getUser is null)
        //        throw new Exception("User not found");

        //    bool verifyPasswords = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);
        //    if (!verifyPasswords)
        //        throw new Exception("Invalid email/password");

        //    string token = GenerateToken(getUser); // method should create

        //    Console.WriteLine("###########: ", token);

        //    return token;
        //}
    }
}
