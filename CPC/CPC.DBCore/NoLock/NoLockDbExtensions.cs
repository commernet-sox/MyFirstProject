using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CPC.DBCore
{
    public static class NoLockDbExtensions
    {
        public static DbContextOptionsBuilder UseNoLock(this DbContextOptionsBuilder builder) => builder.AddInterceptors(new NoLockCommandInterceptor());

        public static IServiceCollection AddNoLockDb<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : DbContext
        {
            services.AddDbContext<T>(o => optionsAction?.Invoke(o), lifetime);
            var item = new ServiceDescriptor(typeof(INoLockDb<T>), _ => CreateNoLockDb<T>(optionsAction), lifetime);
            services.Add(item);
            return services;
        }

        private static INoLockDb<T> CreateNoLockDb<T>(Action<DbContextOptionsBuilder> optionsAction) where T : DbContext
        {
            var optionsBuilder = new DbContextOptionsBuilder<T>();
            optionsAction?.Invoke(optionsBuilder);
            optionsBuilder.UseNoLock();
            var db = Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
            return new NoLockDb<T>(db);
        }
    }
}
