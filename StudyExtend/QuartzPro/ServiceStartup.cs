using CPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AspectCore.Extensions.DependencyInjection;
using CPC.DBCore;
using AspectCore.DependencyInjection;
using QuartzPro.TestApiContexts;
using QuartzPro.Services;
using CPC.Logger;
using CPC.TaskManager.Plugins;
using CPC.TaskManager;
using QuartzPro.Jobs;
using System.Linq;

namespace QuartzPro
{
    internal class ServiceStartup
    {
        private readonly string[] _args;
        public ServiceStartup(string[] args)
        {
            _args = args;
        }
        #region Methods
        internal void Start()
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("app.json").Build();
            Singleton<IConfiguration>.Instance = config;

            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient();
            services.AddDbContext<TestApiContext>(o => o.EnableSensitiveDataLogging(true).UseSqlServer(config.GetConnectionString("TestApiConnection")), ServiceLifetime.Transient);

            var sc = services.ToServiceContext();
            sc.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Transient);

            //sc.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);

            sc.AddType<TestApiService>();
            

            
            sc.AddNLogger();
            sc.AddViewPlugin();
            EngineContext.Initialize(true, null, sc);

            //LogUtility.Fatal("SERVICE START");

            var jobTypes = new[] { typeof(SyncTestApiJob) };

            //var jobTypes = new[] { typeof(SyncYUNDATraceInfoJob) };
            var cron = Singleton<IConfiguration>.Instance.GetSection("SyncOrderTasktime").Value;

            var list = new List<JobContext>();
            jobTypes.ToList().ForEach(t => list.Add(new JobContext { JobType = t, CronExpression = cron }));
            
            TaskPool.StartAsync(list).Wait();
        }

        internal void Stop()
        {
            TaskPool.ShutdownAsync().Wait();
        }
        #endregion
    }
}
