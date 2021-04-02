using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CPC.DBCore;
using Microsoft.EntityFrameworkCore;

namespace DBCoreTest
{
    public class TestApiService : BaseService<TestApiContext,TestApi,TestApiDTO>
    {
        public TestApiService(IRepository<TestApiContext, TestApi> repository, IMapper mapper) : base(repository, mapper)
        {

        }
    }
}
