using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;

namespace authentication.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee?> CreateEmployee(EmployeeCreateReqDTO employeeCreateDTO);

        Task<Employee?> UpdateEmployee(int id, EmployeeUpdateReqDTO employeeUpdateDTO);

        Task<Employee?> GetSingleEmployeeById(int id);

        Task<Employee?> DeleteEmployeeById(int id);

        Task<List<Employee>?> GetAllEmployees();
    }
}
