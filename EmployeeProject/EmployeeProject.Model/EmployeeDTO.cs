namespace EmployeeProject.Model
{
    public class EmployeeDTO
    {
        public int EmpID { get; set; }
        public string EmpCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Salary { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public string Department { get; set; }
    }
}
