using authentication.DbConnections;
using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace authentication.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _db;

        public EmployeeService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
        }

        // Create user
        public async Task<Employee?> CreateEmployee(EmployeeCreateReqDTO employeeCreateDTO)
        {
            
            var user = new Employee()
            {
                Name = employeeCreateDTO.Name,
                Email = employeeCreateDTO.Email,
                Address = employeeCreateDTO.Address,
            };

            _db.Employees.Add(user);
            await _db.SaveChangesAsync();

            return user;
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
        public async Task<Employee?> UpdateEmployee(int id, EmployeeUpdateReqDTO employeeUpdateDTO)
        {
            var response = new EmployeeRes();

            var existingUser = await _db.Employees.FindAsync(id);
            if (existingUser == null) return null;
            

            existingUser.Name = employeeUpdateDTO.Name == null ? existingUser.Name : employeeUpdateDTO.Name;
            existingUser.Address = employeeUpdateDTO.Address == null ? existingUser.Address : employeeUpdateDTO.Address;

            _db.SaveChanges();

            return existingUser;
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
            var employees = await _db.Employees.ToListAsync();
            return employees;
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
