using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QuartzPro.TestApiContexts
{
    public class TestApiContext : DbContext
    {
        public TestApiContext(DbContextOptions<TestApiContext> options) : base(options)
        {
            
        }
        public DbSet<TestApi> TestApi { get; set; }
    }
}
