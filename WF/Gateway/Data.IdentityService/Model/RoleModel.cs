namespace Data.IdentityService.Model
{
    public class RoleModel : BaseModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] SysCode { get; set; }
    }

    public class RoleMenuModel
    {
        public SysRoleDTO Role { get; set; }

        public SysRolemenuDTO[] Menus { get; set; }
    }
}
