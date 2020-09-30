using System;
using AutoMapper;

namespace SDT.BaseTool
{
    public interface IMapperConf
    {
        void Configure(IMapperConfigurationExpression mapper);

        int Order { get; }
    }
}
