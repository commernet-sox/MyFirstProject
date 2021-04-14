using CPC;
using CPC.DBCore;
using Microsoft.EntityFrameworkCore;
using PollyTest1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1
{
    public class PollyTestDbContext : AuditDbContext
    {
        public PollyTestDbContext(DbContextOptions<PollyTestDbContext> options) : base(options)
        {
            
        }
        public DbSet<TestApi> TestApi { get; set; }
        public DbSet<SZCompanyInfo> SZCompanyInfo { get; set; }
        //public override int SaveChanges()
        //{
        //    Audit audit = new Audit();
        //    audit.PreSaveChanges(this);
        //    var rowAffecteds = base.SaveChanges();
        //    audit.PostSaveChanges();

        //    this.AuditEntry.AddRange(audit.Entries);
        //    base.SaveChanges();
        //    if (audit.Configuration.AutoSavePreAction != null && !audit.Entries.IsNull())
        //    {

        //        audit.Configuration.AutoSavePreAction(this, audit);
        //        this.AuditEntry.AddRange(audit.Entries);
        //        base.SaveChanges();
        //    }

        //    return rowAffecteds;
        //}
    }
}
