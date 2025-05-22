using System.Data;
using EmployeeProject.Model;

namespace EmployeeProject.Services
{
    public class DataMapper
    {
        public static List<EmployeeDTO> MapToEmployees(DataTable table)
        {
            try
            {
                var employees = new List<EmployeeDTO>();
                foreach (DataRow row in table.Rows)
                {
                    employees.Add(MapToEmployee(row));
                }
                return employees;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        public static EmployeeDTO MapToEmployee(DataRow row)
        {
            try
            {
                var emp = new EmployeeDTO
                {
                    EmpID = row.Field<int>("EmpID"),
                    Name = row.Field<string>("Name"),
                    Email = row.Field<string>("Email"),
                    EmpCode = row.Field<string>("EmpCode"),
                    Salary = row.Field<int>("Salary"),
                    Mobile = row.Field<string>("Mobile"),
                    CreatedOn = row.Field<DateTime>("CreatedOn"),
                    ModifiedOn = row.Field<DateTime>("ModifiedOn"),
                    Department = row.Field<string>("Department"),
                    Gender = row.Field<string>("Gender"),
                    IsActive = row.Field<bool>("IsActive"),
                };
                return emp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
