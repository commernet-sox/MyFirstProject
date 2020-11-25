using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysRoleService : BaseService<AMSContext, SysRole, SysRoleDTO>
    {
        public SysRoleService(IRepository<AMSContext, SysRole> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
