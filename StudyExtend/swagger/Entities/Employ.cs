using CPC.DependencyInjection;
using CPC.EventBus;
using System;
using System.Threading.Tasks;

namespace swagger
{
    public class Employ : IMapEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }

        
    }
}
