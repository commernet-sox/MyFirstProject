using Data.IdentityService.DTO;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Application.Service
{
    public class SysUserSystemService : BaseService<AMSContext, SysUserSystem, SysUserSystemDTO>
    {
        public SysUserSystemService(IRepository<AMSContext, SysUserSystem> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
