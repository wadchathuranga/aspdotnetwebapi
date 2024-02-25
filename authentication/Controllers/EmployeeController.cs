using authentication.DTOs;
using authentication.Models;
using authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }

        [HttpPost("create")]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}")]
        public async Task<ActionResult<List<Employee?>>> Register(EmployeeCreateDTO employeeCreateDTO)
        {
            var response = await _employeeService.CreateEmployee(employeeCreateDTO);
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}, {UserRoles.USER}")]
        public async Task<ActionResult<Employee?>> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployees();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}, {UserRoles.USER}")]
        public async Task<ActionResult<Employee?>> GetSingleEmployeeById(int id)
        {
            var response = await _employeeService.GetSingleEmployeeById(id);
            if (response is null) return NotFound("User Not Found!");
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}")]
        public async Task<ActionResult<Employee?>> UpdateEmployee(int id, EmployeeUpdateDTO employeeUpdateDTO)
        {
            var response = await _employeeService.UpdateEmployee(id, employeeUpdateDTO);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}")]
        public async Task<ActionResult<Employee?>> DeleteEmployeeById(int id)
        {
            var response = await _employeeService.DeleteEmployeeById(id);
            return Ok(response);
        }
    }
}
