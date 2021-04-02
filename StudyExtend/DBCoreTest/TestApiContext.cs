using System;
using System.Collections.Generic;
using System.Text;
using CPC.DBCore;
using CPC.DBCore.QueryFilter;
using Microsoft.EntityFrameworkCore;

namespace DBCoreTest
{
    public class TestApiContext : DbContext
    {
        public TestApiContext(DbContextOptions<TestApiContext> options) : base(options)
        {
            
        }

        public DbSet<TestApi> TestApi { get; set; }
    }
}
