using EmployeeProject.Model;

namespace EmployeeProject.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<bool> AddEmployeeAsync(EmployeeDTO emp);
        Task<List<EmployeeDTO>> GetAllEmployeesAsync();
        Task<EmployeeDTO> GetEmployeeByIdAsync(int empId);
        Task<bool> UpdateEmployeeAsync(EmployeeDTO emp);
        Task<bool> DeleteEmployeeAsync(int empId);
    }
}
