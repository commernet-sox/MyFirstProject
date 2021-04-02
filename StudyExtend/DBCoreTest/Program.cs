using Microsoft.EntityFrameworkCore;
using System;
using CPC.DBCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System.Linq;
using AspectCore.DependencyInjection;
using CPC;
using AutoMapper;

namespace DBCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //直接生成数据库操作对象
            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //var dbContext= new TestApiContext(optionBuilder.Options);
            //var testApi = dbContext.TestApi.FirstOrDefault();
            //Console.WriteLine(testApi.Name+":"+testApi.Age);
            
            //把对象放入容器中
            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //var dbContext = new TestApiContext(optionBuilder.Options);
            
            //var service = new ServiceContext();
            ////service.AddDelegate<TestApiContext>(_=>dbContext, Lifetime.Scoped);
            //service.AddInstance<TestApiContext>(dbContext);
            //var scope = service.Build();
            //var db = scope.Resolve<TestApiContext>();
            //var testApi = db.TestApi.FirstOrDefault();
            //Console.WriteLine(testApi.Name + ":" + testApi.Age);

            //用仓储模式
            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //var dbContext = new TestApiContext(optionBuilder.Options);
            //var service = new ServiceContext();
            //service.AddInstance(dbContext);
            //service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            //service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //service.AddType<TestApiService>(Lifetime.Scoped);
            ////注入mapper
            //var mapperConf = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<TestApi,TestApiDTO>();
            //    cfg.CreateMap<TestApiDTO,TestApi>();
            //});

            //var mapper = mapperConf.CreateMapper();
            //service.AddInstance(mapper);

            //var scope = service.Build();
            //var testApiService = scope.Resolve<TestApiService>();
            //var testApi = testApiService.Query.FirstOrDefault();
            //Console.WriteLine(testApi.Name + ":" + testApi.Age);

            //CPC写法
            var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            var dbContext = new TestApiContext(optionBuilder.Options);

            var service = new ServiceContext();
            service.AddInstance(dbContext);
            service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            service.AddType<TestApiService>(Lifetime.Scoped);

            var engineSettings = new EngineSettings() { AutoMapperAssemblies=new[] { "DBCoreTest.dll"} };
            EngineContext.Initialize(true, engineSettings, service);

            var scope = service.Build();
            //var testApiService = scope.Resolve<TestApiService>();
            //var testApi = testApiService.Query.FirstOrDefault();
            //Console.WriteLine(testApi.Name + ":" + testApi.Age);

            var scope1 = GlobalContext.CreateScope();
            var testService = scope1.Resolve<TestApiService>();
            var testApi = testService.Query.FirstOrDefault();
            Console.WriteLine(testApi.Name + ":" + testApi.Age);
            Console.ReadKey();
        }
    }
}
