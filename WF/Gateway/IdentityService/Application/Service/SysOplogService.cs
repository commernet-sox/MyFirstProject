using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysOplogService : BaseService<AMSContext, SysOplog, SysOplogDTO>
    {
        public SysOplogService(IRepository<AMSContext, SysOplog> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
