using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysSystemService : BaseService<AMSContext, SysSystem, SysSystemDTO>
    {
        public SysSystemService(IRepository<AMSContext, SysSystem> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
