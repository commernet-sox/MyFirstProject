using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreTest
{
    /// <summary>
    /// 加入审计功能，继承AuditDbContext，不加审计直接继承DbContext
    /// </summary>
    public class TestApiContext : DbContext
    {
        public TestApiContext(DbContextOptions<TestApiContext> options) : base(options)
        {

        }

        public DbSet<TestApi> TestApi { get; set; }

        

    }
}
