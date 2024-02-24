using authentication.DTOs;
using authentication.Models;
using authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) {
            _employeeService = employeeService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<List<Employee?>>> Register(EmployeeCreateDTO employeeCreateDTO)
        {
            var response = await _employeeService.CreateEmployee(employeeCreateDTO);
            return Ok(response);
        }

        //[HttpPost("login")]
        //public async Task<ActionResult> Login(LoginDTO loginDTO)
        //{
        //    var response = await _employeeService.Login(loginDTO);
        //    return Ok(response);
        //}

        [HttpGet]
        public async Task<ActionResult<Employee?>> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployees();
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee?>> UpdateEmployee(int id, EmployeeUpdateDTO employeeUpdateDTO)
        {
            var response = await _employeeService.UpdateEmployee(id, employeeUpdateDTO);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee?>> GetSingleEmployeeById(int id)
        {
            var response = await _employeeService.GetSingleEmployeeById(id);
            if(response is null) return NotFound("User Not Found!");
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee?>> DeleteEmployeeById(int id)
        {
            var response = await _employeeService.DeleteEmployeeById(id);
            return Ok(response);
        }
    }
}
