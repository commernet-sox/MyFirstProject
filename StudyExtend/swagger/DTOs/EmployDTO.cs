using CPC.DependencyInjection;
namespace swagger.DTOs
{
    [MapperProperty(EntityNames = new[] { "swagger.Entities.Company","swagger.Company","Employ" })]
    public class EmployDTO:IMapDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Desc { get; set; }
    }
}
