using Core.Database.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.Models;

namespace WFWebProject
{
    public class DatabaseUniofwork : UnitOfWork<DataContext>
    {
        public DatabaseUniofwork(IHttpContextAccessor contentAccessor, ILoggerFactory logger)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(Core.Infrastructure.Global.DBRWManager.GetMaster(optionsBuilder.Options.ContextType.ToString()));//默认设置为主，查询的时候会自动转到从
            //optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(logger);
            base.DbContext = new DataContext(optionsBuilder.Options);
            
        }
    }
}
