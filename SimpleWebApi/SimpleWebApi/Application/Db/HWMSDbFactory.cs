using CPC.DBCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SimpleWebApi.Application.Cache;
using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Db
{
    public class HWMSDbFactory : IHWMSDbFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public HWMSDbFactory(IHttpContextAccessor accessor, IConfiguration configuration)
        {
            _httpContextAccessor = accessor;
            _configuration = configuration;
        }

        public SimpleWebApiContext Create()
        {
            var builder = CreateDbOptions();
            return new SimpleWebApiContext(builder.Options);
        }

        public SimpleWebApiContext CreateWithNoLock()
        {
            var builder = CreateDbOptions();
            builder.UseNoLock();
            return new SimpleWebApiContext(builder.Options);
        }


        protected virtual DbContextOptionsBuilder<SimpleWebApiContext> CreateDbOptions()
        {
            var cfgId = _httpContextAccessor.HttpContext.GetHeaderValue("CfgId");
            var connectionString = _configuration.GetConnectionString("DB_" + cfgId);

            var optionsBuilder = new DbContextOptionsBuilder<SimpleWebApiContext>();
            return optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
        }
    }
}
