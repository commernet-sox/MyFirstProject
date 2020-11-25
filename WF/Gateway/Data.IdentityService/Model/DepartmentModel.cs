namespace Data.IdentityService.Model
{
    public class DepartmentModel : BaseModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string CompanyId { get; set; }
        public string ParentId { get; set; }
    }
}
