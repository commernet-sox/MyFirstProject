using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysEmployeeService : BaseService<AMSContext, SysEmployee, SysEmployeeDTO>
    {
        public SysEmployeeService(IRepository<AMSContext, SysEmployee> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
