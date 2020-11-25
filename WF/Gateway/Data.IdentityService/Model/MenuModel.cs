using System;

namespace Data.IdentityService.Model
{
    public class MenuModel : BaseModel
    {
        public string MID { get; set; }
        public string MName { get; set; }
        public string SysCode { get; set; }
    }

    public class MenuCommandModel
    {
        public SysMenuDTO Menu { get; set; }

        public SysCommandDTO[] Commands { get; set; }
    }

    public class MenuItemModel 
    {
        public string Mid { get; set; }
        public string Pid { get; set; }
        public string Mcode { get; set; }
        public string Mname { get; set; }
        public string CommandCode { get; set; }
        public string CommandName { get; set; }
        public ulong IsLastLevel { get; set; }
        public string Type { get; set; }

        public MenuItemModel[] items { get; set; }
    }

    public class UserRoleMenuModel
    {
        public string MID { get; set; }
        public string PID { get; set; }
        public string MCode { get; set; }
        public string MName { get; set; }
        public string CommandCode { get; set; }
        public string CommandName { get; set; }
        public string OpenName { get; set; }
    }
}
