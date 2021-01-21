using CPC;
using CPC.DBCore.QueryFilter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SimpleWebApi.Core.Entities;
using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Core
{
    public class DbHost : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //QueryFilterManager.Filter<EntrustOrder>(q => q.Where(t => !t.IsDeleted));
            //QueryFilterManager.Filter<EntrustGoods>(q => q.Where(t => !t.IsDeleted));
            //QueryFilterManager.Filter<Assemble>(q => q.Where(t => !t.IsDeleted));
            //QueryFilterManager.Filter<Bill>(q => q.Where(t => !t.IsDeleted));
            //QueryFilterManager.Filter<EntrustTrack>(q => q.Where(t => !t.IsDeleted));
            //QueryFilterManager.Filter<Region>(q => q.Where(t => !t.IsDeleted));
            QueryFilterManager.Filter<TestApi>(q => q.Where(t => !t.IsDeleted));
            var db = GlobalContext.Resolve<SimpleWebApiContext>();
            db.Database.Migrate();
            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
