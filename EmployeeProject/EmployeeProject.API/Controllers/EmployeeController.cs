using EmployeeProject.Model;
using EmployeeProject.Services.EmployeeServices;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employee)
        {
            var success = await _employeeService.AddEmployeeAsync(employee);

            if (!success)
                return BadRequest("Failed to add employee.");

            return Ok("Employee added successfully.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound("Employee not found.");

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDTO employee)
        {
            var success = await _employeeService.UpdateEmployeeAsync(employee);
            if (!success)
                return NotFound("Employee not found.");

            return Ok("Employee updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
                return NotFound("Employee not found.");

            return Ok("Employee deleted successfully.");
        }
    }
}
