using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace CPC
{
    public class AutoMapperConf : IMapperConf
    {
        #region Members
        public int Order => int.MaxValue;

        protected readonly string[] _assemblyNames;
        #endregion

        #region Constructors
        public AutoMapperConf(string[] assemblyNames) => _assemblyNames = assemblyNames;
        #endregion

        public void Configure(IMapperConfigurationExpression mapper)
        {
            var dtoTypes = TypeFinderUtility.FindClassesOfType<IMapDto>(_assemblyNames);
            var entityTypes = TypeFinderUtility.FindClassesOfType<IMapEntity>(_assemblyNames);

            foreach (var type in dtoTypes)
            {
                var notMap = type.GetCustomAttribute<MapperIgnoreAttribute>(false);
                if (notMap != null)
                {
                    continue;
                }

                var dto = type.Name;
                var entity = string.Empty;

                var map = type.GetCustomAttribute<MapperPropertyAttribute>(true);

                Type entityType = null;
                if (map != null)
                {
                    if (map.MapType != null)
                    {
                        entityType = entityTypes.FirstOrDefault(t => t == map.MapType);
                    }
                    entity = map.MapName;
                }

                if (entityType == null)
                {
                    if (entity.IsNull())
                    {
                        entity = dto.Replace("Dto", "");
                        entity = entity.Replace("DTO", "");
                    }

                    entityType = entityTypes.FirstOrDefault(t => t.Name == entity || t.FullName == entity);
                }

                if (entityType.IsNull())
                {
                    continue;
                }

                mapper.CreateMap(type, entityType);
                mapper.CreateMap(entityType, type);
            }
        }
    }
}
