namespace Data.IdentityService.Model
{
    public class EmployeeModel : BaseModel
    {
        public string CompanyId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
