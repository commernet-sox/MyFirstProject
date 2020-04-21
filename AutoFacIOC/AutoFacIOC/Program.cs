using System;
using Autofac;
namespace AutoFacIOC
{
    class Program
    {
        public static IContainer container;
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleOutput>().AsSelf().As<IOutPut>();
            builder.RegisterType<TodayWrite>().AsSelf().As<IDateWriter>();
            container = builder.Build();
            Console.WriteLine("Hello World!");
            WriteDate();
            Console.ReadKey();
        }
        public static void WriteDate()
        {
            // Create the scope, resolve your IDateWriter,
            // use it, then dispose of the scope.
            using var scope = container.BeginLifetimeScope();
            var writer = scope.Resolve<IDateWriter>();
            writer.WriteData();
        }
    }
}
