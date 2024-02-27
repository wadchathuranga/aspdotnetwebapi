using authentication.DTOs;
using authentication.DTOs.Response;
using authentication.Models;
using authentication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

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
        public async Task<ActionResult<EmployeeRes?>> Create(EmployeeCreateReqDTO employeeCreateDTO)
        {
            var response = new EmployeeRes();

            var result = await _employeeService.CreateEmployee(employeeCreateDTO);

            response.Data = result;
            response.isSucceed = true;
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}, {UserRoles.USER}")]
        public async Task<ActionResult<Employee?>> GetAllEmployees()
        {
            var result = await _employeeService.GetAllEmployees();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}, {UserRoles.USER}")]
        public async Task<ActionResult<EmployeeRes>> GetSingleEmployeeById(int id)
        {
            var response = new EmployeeRes();

            var result = await _employeeService.GetSingleEmployeeById(id);

            if (result is null) 
            {
                response.Error = "Employee Not Found!";
                response.isSucceed = false;
                return NotFound(response); 
            }

            response.Data = result;
            response.isSucceed = true;
            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}, {UserRoles.ADMIN}")]
        public async Task<ActionResult<EmployeeRes>> UpdateEmployee(int id, EmployeeUpdateReqDTO employeeUpdateDTO)
        {
            var response = new EmployeeRes();

            var result = await _employeeService.UpdateEmployee(id, employeeUpdateDTO);

            if (result == null)
            {
                response.Error = "Employee Not Found!";
                response.isSucceed = false;
                return NotFound(response);
            }

            response.Data = result;
            response.isSucceed = true;
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{UserRoles.OWNER}")]
        public async Task<ActionResult<EmployeeRes?>> DeleteEmployeeById(int id)
        {
            var response = new EmployeeRes();

            var result = await _employeeService.DeleteEmployeeById(id);

            response.Data = result;
            response.isSucceed = true;
            return Ok(response);
        }
    }
}
