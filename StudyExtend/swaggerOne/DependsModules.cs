using System;
using System.Collections.Generic;
using System.Text;
using AspectCore.DependencyInjection;
using CPC.DependencyInjection;
using CPC;
using swaggerOne.Interfaces;
using swaggerOne.Services;

namespace swaggerOne
{
    internal class DependsModules : IDependsModule
    {
        public void Register(IServiceContext services)
        {
            services.AddType<ISchool,SchoolServiec>();
        }
    }
}
