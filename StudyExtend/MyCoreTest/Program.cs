using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using Core.DBCore;
using Core.DBCore.Repository;
using AspectCore.DependencyInjection;
using Core.DependencyInjection;
using System.Linq;

namespace MyCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //CPC写法
            var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            var connectionString = "server=47.98.229.13;database=CompanyInfo;uid=sa;password=123qwe!@#";
            var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            var dbContext = new TestApiContext(optionBuilder.Options);

            var service = new ServiceContext();
            service.AddInstance(dbContext);
            service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            service.AddType<TestApiService>(Lifetime.Scoped);

            var engineSettings = new EngineSettings() { AutoMapperAssemblies = new[] { "MyCoreTest.dll" } };
            EngineContext.Initialize(true, engineSettings, service);

            var scope = service.Build();
            //var testApiService = scope.Resolve<TestApiService>();
            //var testApi = testApiService.Query.FirstOrDefault();
            //Console.WriteLine(testApi.Name + ":" + testApi.Age);

            var scope1 = GlobalContext.CreateScope();
            var testService = scope1.Resolve<TestApiService>();
            var testApi = testService.Query.FirstOrDefault();
            Console.WriteLine(testApi.Name + ":" + testApi.Age);
            //下一步把包放到服务器上下载。

            Console.ReadKey();
        }
    }
}
