using EmployeeProject.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeProject.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration configuration)
        {
            _connectionString = configuration?.GetConnectionString("DefaultConnection");
        }
        public async Task<bool> AddEmployeeAsync(EmployeeDTO emp)
        {
            using var conn = new SqlConnection(_connectionString);
            const string query = @"
                INSERT INTO Employees (EmpCode, Name, Salary, Mobile, Email, CreatedOn, ModifiedOn)
                VALUES (@EmpCode, @Name, @Salary, @Mobile, @Email, @CreatedOn, @ModifiedOn)";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@EmpCode", emp.EmpCode ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", emp.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@Mobile", emp.Mobile ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", emp.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesAsync()
        {
            const string query = "SELECT * FROM Employees";
            var employees = new List<EmployeeDTO>();
            employees = await ReadDataAsync(query);
            return employees;
        }
        public async Task<EmployeeDTO> GetEmployeeByIdAsync(int empId)
        {
            //EmployeeDTO employee = null;
            const string query = "SELECT * FROM Employees WHERE EmpID = @EmpID";
            var employees = await ReadDataAsync(query, new Dictionary<string, object> { { "@EmpID", empId } });
            return employees?.FirstOrDefault();
        }
        public async Task<bool> UpdateEmployeeAsync(EmployeeDTO emp)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                const string query = @"
                    UPDATE Employees SET
                        EmpCode = @EmpCode,
                        Name = @Name,
                        Salary = @Salary,
                        Mobile = @Mobile,
                        Email = @Email,
                        ModifiedOn = @ModifiedOn
                    WHERE EmpID = @EmpID";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpCode", emp.EmpCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", emp.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Salary", emp.Salary);
                    cmd.Parameters.AddWithValue("@Mobile", emp.Mobile ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", emp.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);
                    cmd.Parameters.AddWithValue("@EmpID", emp.EmpID);
                    await conn.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }
        public async Task<bool> DeleteEmployeeAsync(int empId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                const string query = "DELETE FROM Employees WHERE EmpID = @EmpID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpID", empId);
                    await conn.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }
        private async Task<List<EmployeeDTO>> ReadDataAsync(string query, Dictionary<string, object>? parameters = null)
        {
            var employees = new List<EmployeeDTO>();
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    await conn.OpenAsync();
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            employees.Add(MapEmployee(reader));
                        }
                    }
                }
            }
            return employees;
        }
        private EmployeeDTO MapEmployee(SqlDataReader reader)
        {
            return new EmployeeDTO
            {
                EmpID = reader.GetInt32(0),
                EmpCode = reader.GetString(1),
                Name = reader.GetString(2),
                Salary = reader.GetInt32(3),
                Mobile = reader.GetString(4),
                Email = reader.GetString(5),
                CreatedOn = reader.GetDateTime(6),
                ModifiedOn = reader.GetDateTime(7)
            };
        }
    }
}
