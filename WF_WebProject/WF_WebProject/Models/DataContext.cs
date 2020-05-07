using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.Models;

namespace WFWebProject.Models
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<WFWebProject.Models.CodeMaster> CodeMaster { get; set; }
    }
}
