using authentication.DTOs;
using authentication.Models;

namespace authentication.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> CreateEmployee(EmployeeCreateDTO employeeCreateDTO);

        //Task<string> Login(LoginDTO loginDTO);

        Task<List<Employee>?> UpdateEmployee(int id, EmployeeUpdateDTO employeeUpdateDTO);

        Task<Employee?> GetSingleEmployeeById(int id);

        Task<Employee?> DeleteEmployeeById(int id);

        Task<List<Employee>?> GetAllEmployees();
    }
}
