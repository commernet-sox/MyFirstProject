using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysCompanyService : BaseService<AMSContext, SysCompany, SysCompanyDTO>
    {
        public SysCompanyService(IRepository<AMSContext, SysCompany> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
