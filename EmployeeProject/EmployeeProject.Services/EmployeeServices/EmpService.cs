using System.Data;
using EmployeeProject.Model;
using EmployeeProject.Services.HelperService;
using Microsoft.Data.SqlClient;

namespace EmployeeProject.Services.EmployeeServices
{
    public class EmpService : IEmployeeService
    {
        private IAdoHelper _adoHelper;
        public EmpService(IAdoHelper adoHelper)
        {
            _adoHelper = adoHelper; ;
        }
        public async Task<bool> AddEmployeeAsync(EmployeeDTO emp)
        {
            string insertSql = $@"INSERT INTO Employees (EmpCode, Name, Salary, Mobile, Email, CreatedOn, ModifiedOn, IsActive, Gender, Department)
                VALUES ({Constants.EmpCode}, @Name, @Salary, @Mobile, @Email, @CreatedOn, @ModifiedOn, @IsActive, @Gender, @Department)";
            var insertParams = new[]
                    {
                    new SqlParameter(Constants.EmpCode, emp.EmpCode ?? (object)DBNull.Value),
                    new SqlParameter("@Name", emp.Name ?? (object)DBNull.Value),
                    new SqlParameter("@Salary", emp.Salary),
                    new SqlParameter("@Mobile", emp.Mobile ?? (object)DBNull.Value),
                    new SqlParameter("@Email", emp.Email ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedOn", DateTime.Now),
                    new SqlParameter("@ModifiedOn", DateTime.Now),
                    new SqlParameter("@IsActive", emp.IsActive),
                    new SqlParameter("@Gender",emp.Gender?? (object)DBNull.Value),
                    new SqlParameter("@Department", emp.Department?? (object)DBNull.Value)
                   };
            return await _adoHelper.ExecuteNonQueryAsync(insertSql, insertParams) > 0;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            string getSql = "SELECT * FROM Employees";
            DataTable result = await _adoHelper.ExecuteQueryAsync(getSql, null);
            if (result != null && result.Rows.Count > 0)
                return DataMapper.MapToEmployees(result);
            else
                return new List<EmployeeDTO>();
        }
        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int empId)
        {
            string getSql = "SELECT * FROM Employees WHERE EmpID = @EmpID";
            var getParams = new[] { new SqlParameter("@EmpID", empId) };
            DataTable result = await _adoHelper.ExecuteQueryAsync(getSql, getParams);
            if (result != null && result.Rows.Count > 0)
                return DataMapper.MapToEmployee(result.Rows[0]);
            else
                return null;
        }
        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO emp)
        {
            string updateSql = $@"
                    UPDATE Employees SET
                        EmpCode = {Constants.EmpCode},
                        Name = @Name,
                        Salary = @Salary,
                        Mobile = @Mobile,
                        Email = @Email,
                        ModifiedOn = @ModifiedOn,
                        IsActive   = @IsActive,
                        Gender = @Gender,
                        Department =@Department
                    WHERE EmpID = @EmpID";

            var updateParams = new[]
            {
                new SqlParameter(Constants.EmpCode, emp.EmpCode ?? (object)DBNull.Value),
                new SqlParameter("@Name", emp.Name ?? (object)DBNull.Value),
                new SqlParameter("@Salary", emp.Salary),
                new SqlParameter("@Mobile", emp.Mobile ?? (object)DBNull.Value),
                new SqlParameter("@Email", emp.Email ?? (object)DBNull.Value),
                new SqlParameter("@ModifiedOn", DateTime.Now),
                new SqlParameter("@IsActive", emp.IsActive),
                new SqlParameter("@Gender",emp.Gender?? (object)DBNull.Value),
                new SqlParameter("@Department", emp.Department?? (object)DBNull.Value),
                new SqlParameter("@EmpID", emp.EmpID),
            };
            return await _adoHelper.ExecuteNonQueryAsync(updateSql, updateParams) > 0;
        }
        public async Task<bool> DeleteEmployeeAsync(int empId)
        {
            return await _adoHelper.DeleteRecord("Employees", "EmpID", empId);
        }
    }

    public class Constants
    {
        public const string EmpCode = "@EmpCode";
    }
}
