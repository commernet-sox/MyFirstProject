using AspectCore.DependencyInjection;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPC
{
    public class EngineBuilder : IEngine
    {
        #region Members
        public EngineSettings EngineSettings { get; private set; }

        /// <summary>
        /// container
        /// </summary>
        public IServiceResolver Container { get; private set; }

        protected readonly IServiceContext _services;

        protected IEnumerable<IEngineService> _engineServices;
        #endregion

        #region Constructors
        public EngineBuilder(IServiceContext services = null, EngineSettings settings = null)
        {
            EngineSettings = settings ?? new EngineSettings();
            _services = services ?? new ServiceContext();
        }
        #endregion

        #region Public Methods
        public void Initialize() => RegisterDependencies();

        public void Dispose()
        {
            StopService();
            Container.Dispose();
        }

        public void StartService()
        {
            if (_engineServices != null)
            {
                throw new InvalidOperationException("engine service started");
            }

            _engineServices = Container.CreateScope().ResolveMany<IEngineService>();

            foreach (var service in _engineServices)
            {
                try
                {
                    service.Start();
                }
                catch (Exception ex)
                {
                    LogUtility.Error(ex, "EngineService Start");
                }
            }
        }

        public void StopService()
        {
            if (_engineServices.IsNull())
            {
                return;
            }

            foreach (var service in _engineServices)
            {
                try
                {
                    service.Stop();
                }
                catch (Exception ex)
                {
                    LogUtility.Error(ex, "EngineService Stop");
                }
            }

            _engineServices = null;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// register dependencies
        /// </summary>
        protected virtual void RegisterDependencies()
        {
            var drInstances = new List<IDependencyRegistrar>();

            if (!EngineSettings.DependencyAssemblies.IsNull())
            {
                //register dependencies provided by other assemblies
                var drTypes = TypeFinderUtility.FindClassesOfType<IDependencyRegistrar>(EngineSettings.DependencyAssemblies);
                foreach (var drType in drTypes)
                {
                    drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
                }

                //sort
                drInstances = drInstances.OrderBy(t => t.Order).ToList();
            }

            if (!EngineSettings.AutoDependencyAssemblies.IsNull())
            {
                drInstances.Add(new AutoDependencyRegistrar(EngineSettings.AutoDependencyAssemblies));
            }

            foreach (var dependencyRegistrar in drInstances)
            {
                dependencyRegistrar.Register(_services);
            }

            RegisterMapperConf();
            Container = _services.Build();
        }

        protected virtual void RegisterMapperConf()
        {
            var mcInstances = new List<IMapperConf>();
            if (!EngineSettings.MapperAssemblies.IsNull())
            {
                //register mapper configurations provided by other assemblies
                var mcTypes = TypeFinderUtility.FindClassesOfType<IMapperConf>(EngineSettings.MapperAssemblies);
                foreach (var mcType in mcTypes)
                {
                    mcInstances.Add((IMapperConf)Activator.CreateInstance(mcType));
                }

                //sort
                mcInstances = mcInstances.OrderBy(t => t.Order).ToList();
            }

            if (!EngineSettings.AutoMapperAssemblies.IsNull())
            {
                mcInstances.Add(new AutoMapperConf(EngineSettings.AutoMapperAssemblies));
            }

            if (!mcInstances.IsNull())
            {
                var mapperConf = new MapperConfiguration(cfg =>
                {
                    foreach (var mc in mcInstances)
                    {
                        mc.Configure(cfg);
                    }
                });

                var mapper = mapperConf.CreateMapper();
                _services.AddInstance(mapper);
            }
        }
        #endregion
    }
}
