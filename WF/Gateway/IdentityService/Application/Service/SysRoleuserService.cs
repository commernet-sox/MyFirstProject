using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Service
{
    public class SysRoleuserService : BaseService<AMSContext, SysRoleuser, SysRoleuserDTO>
    {
        public SysRoleuserService(IRepository<AMSContext, SysRoleuser> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
