using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Collections.Generic;

namespace CPC.Service
{
    [Route("[controller]"), ApiExplorerSettings(IgnoreApi = true)]
    public class BaseSwaggerApiController : RestApiController
    {
        public ActionResult<List<string>> Get([FromServices] IApiVersionDescriptionProvider provider)
        {
            var list = new List<string>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                list.Add($"/swagger/{description.GroupName}/swagger.json");
            }

          return list;
        }
    }
}
