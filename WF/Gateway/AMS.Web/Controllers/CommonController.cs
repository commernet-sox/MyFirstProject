using Data.IdentityService;
using Data.IdentityService.Model;
using AMS.WebCore;
using CPC;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Web.Controllers
{
    public class CommonController : BaseController
    {
        public List<IDNameDto> GetCompanyList()
        {
            CompanyModel model = new CompanyModel { PageIndex = 0, PageSize = 100 };
            var pageData = Get("/os/1.0/company/getlist", model).As<PageData<List<SysCompanyDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.CompanyId, NAME = item.CompanyName })
                .ToList();
        }

        public List<IDNameDto> GetDepartmentList(string CompanyId)
        {
            DepartmentModel model = new DepartmentModel { PageIndex = 0, PageSize = 100 };
            model.CompanyId = CompanyId;
            var pageData = Get("/os/1.0/department/getlist", model).As<PageData<List<SysDepartmentDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.DepartmentId, NAME = item.DepartmentName })
                .ToList();
        }

        public List<IDNameDto> GetEmployeeList()
        {
            EmployeeModel model = new EmployeeModel { PageIndex = 0, PageSize = 100 };
            var pageData = Get("/os/1.0/employee/getlist", model).As<PageData<List<SysEmployeeDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.EmployeeId, NAME = item.EmployeeName })
                .ToList();
        }

        public List<IDNameDto> GetSystemList()
        {
            SystemModel model = new SystemModel { PageIndex = 0, PageSize = 100 };
            var pageData = Get("/os/1.0/system/getlist", model).As<PageData<List<SysSystemDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.SysCode, NAME = item.SysName })
                .ToList();
        }
        public List<IDNameDto> GetUserList()
        {
            UserModel model = new UserModel { PageIndex = 0, PageSize = 100 };
            var pageData = Get("/os/1.0/user/getlist", model).As<PageData<List<SysUserDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.UserId, NAME = item.PersonName })
                .ToList();
        }
        public List<IDNameDto> GetRoleList()
        {
            RoleModel model = new RoleModel { PageIndex = 0, PageSize = 100,SysCode=new string[] { "AMS" } };
            var pageData = Post("/os/1.0/role/getlist", model).As<PageData<List<SysRoleDTO>>>().Result;
            return pageData.Data
                .Select(item => new IDNameDto { ID = item.RoleId, NAME = item.RoleName })
                .ToList();
        }
    }
}
