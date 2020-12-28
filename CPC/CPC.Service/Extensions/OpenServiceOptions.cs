using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace CPC.Service
{
    public class OpenServiceOptions
    {
        /// <summary>
        /// 默认版本号
        /// </summary>
        public ApiVersion DefaultVersion { get; set; } = new ApiVersion(1, 0);

        /// <summary>
        /// API 版本信息设置
        /// </summary>
        public Action<ApiVersioningOptions> ApiVersionSetup { get; set; }

        /// <summary>
        /// 是否不再生成API文档
        /// </summary>
        public bool IgnoreApiDoc { get; set; } = false;

        /// <summary>
        /// 是否启用Consul
        /// </summary>
        public bool EnableConsul { get; set; } = true;

        /// <summary>
        /// API视图配置
        /// </summary>
        public Action<ApiExplorerOptions> ApiExplorerSetup { get; set; }

        /// <summary>
        /// 文档
        /// </summary>
        public Action<SwaggerGenOptions> SwaggerGenSetup { get; set; }

        /// <summary>
        /// 创建文档信息
        /// </summary>
        public Action<ApiVersionDescription, OpenApiInfo> CreateForApiDesc { get; set; }

        /// <summary>
        /// 项目注释文件
        /// </summary>
        public string[] XmlComments { get; set; }

        /// <summary>
        /// 是否使用json.net 序列化(swagger文档)
        /// </summary>
        public bool SwaggerNewtonsoftSupport { get; set; } = true;
    }
}
