using AutoMapper;
using System.Linq;

namespace CPC
{
    public static class MapperExtensions
    {
        public static T Map<T, TSource>(this TSource source, IMapper mapper = null)
            where T : class
            where TSource : class
        {
            if (mapper == null)
            {
                mapper = GlobalContext.Resolve<IMapper>();
            }

            var result = mapper.Map<T>(source);
            return result;
        }

        public static T Map<T>(this object source, IMapper mapper = null)
            where T : class
        {
            if (mapper == null)
            {
                mapper = GlobalContext.Resolve<IMapper>();
            }

            var result = mapper.Map<T>(source);
            return result;
        }


        public static IQueryable<T> Map<T>(this IQueryable source, IMapper mapper = null)
        {
            if (mapper == null)
            {
                mapper = GlobalContext.Resolve<IMapper>();
            }

            var query = mapper.ProjectTo<T>(source, mapper.ConfigurationProvider);
            return query;
        }


    }
}
