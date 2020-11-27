using AutoMapper;
using CPC.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestData.DTO;
using TestCore.Entities;
using TestInfrastructure;
using TestApi.Application.Core;

namespace TestApi.Application.Services
{
    public class TestApiService : BaseTmsService<TestCore.Entities.TestApi,TestApiDTO>
    {
        public TestApiService(IRepository<TestContext, TestCore.Entities.TestApi> repository, IMapper mapper, IOperate operate) : base(repository, mapper, operate)
        {
        }
    }
}
