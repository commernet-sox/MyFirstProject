using CPC.DBCore.QueryFilter;
using Microsoft.EntityFrameworkCore;
using System;
using TestCore.Entities;

namespace TestInfrastructure
{
    public class TestContext: DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {
            QueryFilterManager.InitilizeGlobalFilter(this);
        }
        public DbSet<TestApi> TestApi { get; set; }
    }
}
