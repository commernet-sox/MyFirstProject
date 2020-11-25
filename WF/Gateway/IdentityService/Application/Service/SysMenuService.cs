using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysMenuService : BaseService<AMSContext, SysMenu, SysMenuDTO>
    {
        public SysMenuService(IRepository<AMSContext, SysMenu> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
