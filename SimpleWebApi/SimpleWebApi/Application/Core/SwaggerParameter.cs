using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Core
{
    public class SwaggerParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

            //先判断是否是匿名访问,
            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);
                bool isAnonymous = actionAttributes.Any(a => a is AllowAnonymousAttribute);

                if (!isAnonymous)
                {
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "UserId",
                        In = ParameterLocation.Header,
                        Description = "用户ID",
                        Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("admin") }
                    });
                    operation.Parameters.Add(new OpenApiParameter()
                    {
                        Name = "CfgId",
                        In = ParameterLocation.Header,
                        Description = "DB",
                        Schema = new OpenApiSchema { Type = "string", Default = new OpenApiString("1") }
                    });
                }
            }

        }
    }
}
