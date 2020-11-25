using Data.IdentityService;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService
{
    public class AppService : BaseService<AMSContext, AuthApp, AuthAppDTO>
    {
        public AppService(IRepository<AMSContext, AuthApp> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
