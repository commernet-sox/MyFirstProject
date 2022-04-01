using CPC.DependencyInjection;
namespace swagger.DTOs
{
    [MapperProperty(EntityNames =new[] { "Employ"})]
    public class CompanyDTO:IMapDto
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string CompanyID { get; set; }
    }
}
