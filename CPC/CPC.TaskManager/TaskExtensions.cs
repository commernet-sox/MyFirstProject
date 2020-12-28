using AspectCore.DependencyInjection;

namespace CPC.TaskManager
{
    public static class TaskExtensions
    {
        public static IServiceContext AddTaskPlugin<T>(this IServiceContext services) where T : TaskPlugin => services.AddType<TaskPlugin, T>(Lifetime.Singleton);

        public static IServiceContext AddTaskPlugin(this IServiceContext services, TaskPlugin plugin) => services.AddInstance(plugin);

    }
}
