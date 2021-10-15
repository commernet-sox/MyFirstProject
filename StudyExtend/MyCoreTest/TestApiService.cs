using AutoMapper;
using Core.DBCore.Repository;
using Core.DBCore.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreTest
{
    public class TestApiService : TableService<TestApiContext, TestApi, TestApiDTO>
    {
        public TestApiService(IRepository<TestApiContext, TestApi> repository):base(repository)
        {

        }
    }
}
