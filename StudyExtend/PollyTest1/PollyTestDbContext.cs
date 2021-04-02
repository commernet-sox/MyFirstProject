using Microsoft.EntityFrameworkCore;
using PollyTest1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1
{
    public class PollyTestDbContext : DbContext
    {
        public PollyTestDbContext(DbContextOptions<PollyTestDbContext> options) : base(options)
        {
            
        }
        public DbSet<TestApi> TestApi { get; set; }
        public DbSet<SZCompanyInfo> SZCompanyInfo { get; set; }
    }
}
