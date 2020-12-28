using AutoMapper;

namespace CPC
{
    public interface IMapperConf
    {
        void Configure(IMapperConfigurationExpression mapper);

        int Order { get; }
    }
}
