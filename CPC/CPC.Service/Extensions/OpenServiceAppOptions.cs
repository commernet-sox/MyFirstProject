using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace CPC.Service
{
    public class OpenServiceAppOptions
    {
        /// <summary>
        /// Swagger配置
        /// </summary>
        public Action<SwaggerOptions> SwaggerSetup { get; set; }

        /// <summary>
        /// Swagger UI配置
        /// </summary>
        public Action<SwaggerUIOptions> SwaggerUISetup { get; set; }
    }
}
