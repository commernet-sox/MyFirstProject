using AspectCore.DependencyInjection;
using System;
using System.Runtime.CompilerServices;

namespace CPC
{
    public static class EngineContext
    {
        #region Public Members
        /// <summary>
        /// gets the singleton engine used to access services.
        /// </summary>
        public static IEngine Current { get; private set; }

        public static bool Initialized => Current != null;
        #endregion

        #region Public Methods
        /// <summary>
        /// initializes a static instance of the factory.
        /// </summary>
        /// <param name="forceRecreate">creates a new factory instance even though the factory has been previously initialized</param>
        /// <param name="settings"></param>
        /// <param name="service"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate, EngineSettings settings = null, IServiceContext service = null) => Initialize(service, settings, forceRecreate);

        /// <summary>
        /// initializes a static instance of the Nop factory.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <param name="forceRecreate">creates a new factory instance even though the factory has been previously initialized</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(IServiceContext services = null, EngineSettings settings = null, bool forceRecreate = false)
        {
            if (Current == null || forceRecreate)
            {
                var builder = new EngineBuilder(services, settings);
                Replace(builder);
            }

            return Current;
        }

        /// <summary>
        /// sets the static engine instance to the supplied engine. use this method to supply your own engine implementation
        /// </summary>
        /// <param name="engine">the engine to use</param>
        public static void Replace(IEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            if (Initialized)
            {
                Current.StopService();
            }

            engine.Initialize();
            Current = engine;
            Current.StartService();
        }

        public static void Dispose() => Current?.Dispose();
        #endregion
    }


}
