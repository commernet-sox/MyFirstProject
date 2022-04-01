using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPCRedisTest
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {

        }

        public DbSet<MultiThread> MultiThread { get; set; }
    }
}
