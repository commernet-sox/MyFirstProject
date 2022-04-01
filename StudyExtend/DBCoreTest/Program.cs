using CPC.DBCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //var dbContext = new TestApiContext(optionBuilder.Options);

            //var service = new ServiceContext();
            //service.AddInstance(dbContext);
            //service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            //service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //service.AddType<TestApiService>(Lifetime.Scoped);

            //var engineSettings = new EngineSettings() { AutoMapperAssemblies=new[] { "DBCoreTest.dll"} };
            //EngineContext.Initialize(true, engineSettings, service);

            //var scope = service.Build();
            ////var testApiService = scope.Resolve<TestApiService>();
            ////var testApi = testApiService.Query.FirstOrDefault();
            ////Console.WriteLine(testApi.Name + ":" + testApi.Age);

            //var scope1 = GlobalContext.CreateScope();
            //var testService = scope1.Resolve<TestApiService>();
            //var testApi = testService.Query.FirstOrDefault();
            //Console.WriteLine(testApi.Name + ":" + testApi.Age);
            //Console.ReadKey();


            //CPC插入100W条数据计时   Bluk

            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=10.27.1.58;database=SIMPLE_TMS;uid=wmsadmin;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //var dbContext = new TestApiContext(optionBuilder.Options);
            //var service = new ServiceContext();
            //service.AddInstance(dbContext);
            //service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            //service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //service.AddType<TestApiService>(Lifetime.Scoped);
            //var engineSettings = new EngineSettings() { AutoMapperAssemblies = new[] { "DBCoreTest.dll" } };
            //EngineContext.Initialize(true, engineSettings, service);
            //var scope = service.Build();
            //var scope1 = GlobalContext.CreateScope();
            //var testService = scope1.Resolve<TestApiService>();
            //var db = scope1.Resolve<TestApiContext>();
            //List<TestApi> testApiDTOs = new List<TestApi>();
            //for (int i = 0; i < 10000; i++)
            //{
            //    TestApi testApiDTO = new TestApi();
            //    testApiDTO.Name = i.ToString();
            //    testApiDTO.Age = i;
            //    testApiDTO.CreateBy = "admin";
            //    testApiDTO.CreateTime = DateTime.Now;
            //    testApiDTOs.Add(testApiDTO);
            //}
            //Stopwatch sw1 = new Stopwatch();
            //sw1.Start();
            //var a = db.Bulk();
            //a.Setup<TestApi>(t => t.ForCollection(testApiDTOs)).WithTable("TestApi").WithBulkCopyBatchSize(4000).AddAllColumn().BulkInsert();
            //a.CommitTrans();
            //sw1.Stop();
            //long time = sw1.ElapsedMilliseconds;
            //Console.WriteLine(time);

            //测试审计   Audit   
            //var optionsBuilder = new DbContextOptionsBuilder<TestApiContext>();
            //var connectionString = "server=47.98.229.13;database=CompanyInfo;uid=sa;password=123qwe!@#";
            //var optionBuilder = optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            //// ---------------NoLock-----------------------
            //optionBuilder.UseNoLock();
            //var dbContext = new TestApiContext(optionBuilder.Options);
            //var service = new ServiceContext();
            //service.AddInstance(dbContext);
            //service.AddType(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifetime.Scoped);
            //service.AddType(typeof(IRepository<,>), typeof(Repository<,>), Lifetime.Scoped);
            //service.AddType<TestApiService>(Lifetime.Scoped);
            //var engineSettings = new EngineSettings() { AutoMapperAssemblies = new[] { "DBCoreTest.dll" } };
            //EngineContext.Initialize(true, engineSettings, service);
            //var scope = service.Build();
            //var scope1 = GlobalContext.CreateScope();
            //var testService = scope1.Resolve<TestApiService>();
            //只更新修改的字段testService.Repository.CreateSet().FirstOrDefault();
            //更新所有字段testService.Query.FirstOrDefault();
            //var org = testService.Repository.CreateSet().FirstOrDefault();
            //org.Name = Guid.NewGuid().ToString("N").Substring(10);
            //testService.DataContext.SaveChanges();


            #region 批量删除更新          Batch
            //testService.Repository.Query.Where(t => t.Id > 1).Update(t => new TestApi() { Name="111",CreateBy="admin"});
            //testService.Repository.Query.Where(t => t.Id > 1).Delete();
            #endregion

            //#region sql缓存   Cache
            //var third1 = testService.DataContext.Database.GetDbConnection().ConnectionString;
            //var first1 = testService.Repository.Query.Where(t => t.Id == 1).Take(1).Future();
            //var first = testService.Repository.Query.Where(t => t.Id == 1).FromCache().FirstOrDefault();
            //var second = testService.Repository.Query.Where(t => t.Id == 1).FromCache().FirstOrDefault();
            //var forth = testService.Query;
            //var third = testService.DataContext.Database.GetDbConnection().ConnectionString;
            //var cache = GetCacheKeys();
            //#endregion
            //Console.WriteLine("完成!");
            //Console.ReadKey();
        }

        public static List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = QueryCacheManager.Cache.GetType().GetField("_entries", flags).GetValue(QueryCacheManager.Cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }
    }
}
