using CPC.DBCore.QueryFilter;
using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Core.Entities;
using System;

namespace SimpleWebApi.Infrastructure
{
    public class SimpleWebApiContext : DbContext
    {
        public SimpleWebApiContext(DbContextOptions<SimpleWebApiContext> options) : base(options)
        {
            QueryFilterManager.InitilizeGlobalFilter(this);
        }
        public DbSet<TestApi> TestApi { get; set; }
    }
}
