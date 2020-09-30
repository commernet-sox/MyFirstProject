using AspectCore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDT.BaseTool
{
    public interface IEngine : IDisposable
    {
        /// <summary>
        /// engine setting
        /// </summary>
        EngineSettings EngineSettings { get; }

        /// <summary>
        /// container
        /// </summary>
        IServiceResolver Container { get; }

        /// <summary>
        /// initialize components and plugins in the environment.
        /// </summary>
        void Initialize();

        void StartService();

        void StopService();
    }
}
