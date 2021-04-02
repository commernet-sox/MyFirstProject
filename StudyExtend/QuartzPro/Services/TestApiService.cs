using CPC.DBCore;
using QuartzPro.TestApiContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuartzPro.Services
{
    public class TestApiService : Repository<TestApiContext, TestApi>
    {
        public TestApiService(IUnitOfWork<TestApiContext> unitOfWork) : base(unitOfWork)
        {

        }


    }
}
