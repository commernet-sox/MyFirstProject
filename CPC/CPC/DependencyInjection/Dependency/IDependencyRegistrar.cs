using AspectCore.DependencyInjection;

namespace CPC
{
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// register services and interfaces
        /// </summary>
        /// <param name="service">service context</param>
        void Register(IServiceContext service);

        /// <summary>
        /// order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
