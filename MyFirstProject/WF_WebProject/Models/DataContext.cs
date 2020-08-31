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
        public DbSet<CompanyInfo> CompanyInfo_new { get; set; }
        public DbSet<CodeMaster> CodeMaster { get; set; }
        public DbSet<CompanyQualification> CompanyQualification_new { get; set; }
        public DbSet<ConstructorInfo> ConstructorInfo_new { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<UserMenuRole> UserMenuRole { get; set; }

    }
}
