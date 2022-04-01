using CPC.DependencyInjection;
using CPC.EventBus;
namespace swagger.Entities
{
    public class Company:IMapEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}
