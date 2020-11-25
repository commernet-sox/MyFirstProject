using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;

namespace IdentityService
{
    public class SysDepartmentService : BaseService<AMSContext, SysDepartment, SysDepartmentDTO>
    {
        public SysDepartmentService(IRepository<AMSContext, SysDepartment> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
