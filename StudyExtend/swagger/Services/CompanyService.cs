using swagger.Interfaces;
using CPC.DependencyInjection;
using CPC;
using AspectCore.DependencyInjection;
using CPC.Logging;
using System;
using CPC.EventBus;
using CPC.Extensions;
using swagger.Entities;
using System.Linq;

namespace swagger.Services
{
    public class CompanyService : ICompany,IAutoDepends
    {
        public void CompanyName(string name)
        {
            var scope = EngineContext.CreateScope();
            var empService = scope.Resolve<IEmploy>();
            //var elog = scope.Resolve<ILogger>();
            //var rabbitEventBus = scope.Resolve<RabbitEventBus>();
            //rabbitEventBus.Subscribe<CompanyProducer, CompanyConsumer>();
            //foreach (var item in Enumerable.Range(0, 100))
            //{
            //    CompanyProducer producer = new CompanyProducer();
            //    producer.Name = item.ToString();
            //    rabbitEventBus.Publish(producer);
            //}
            LogUtility.Info("111");
            
            empService.Name(name);
        }
    }
}
