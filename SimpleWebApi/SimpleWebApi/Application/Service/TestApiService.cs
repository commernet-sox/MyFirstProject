using AutoMapper;
using CPC.DBCore;
using SimpleWebApi.Application.Core;
using SimpleWebApi.Core.Entities;
using SimpleWebApi.Data.DTO;
using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Service
{
    public class TestApiService : BaseService<TestApi, TestApiDTO>
    {
        
        public TestApiService(IRepository<SimpleWebApiContext, TestApi> repository, IMapper mapper, IOperate operate) : base(repository, mapper,operate)
        {
            
        }
    }
}
