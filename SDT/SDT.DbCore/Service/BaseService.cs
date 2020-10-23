using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SDT.DbCore
{
    public class BaseService<TDbContext, TEntity, TDto> :
        AbstractService<TDbContext, TEntity, TDto>
        where TDbContext : DbContext
        where TEntity : class
        where TDto : class
    {
        protected IMapper Mapper { get; set; }

        public BaseService(IRepository<TDbContext, TEntity> repository, IMapper mapper) : base(repository) => Mapper = mapper;

        protected override TEntity Transfer(TDto dto)
        {
            var entity = Mapper.Map<TEntity>(dto);
            return entity;
        }

        protected override TDto Transfer(TEntity entity)
        {
            var dto = Mapper.Map<TDto>(entity);
            return dto;
        }

        protected override IQueryable<TEntity> Transfer(IQueryable<TDto> dtos)
        {
            var query = Mapper.ProjectTo<TEntity>(dtos, Mapper.ConfigurationProvider);
            return query;
        }

        protected override IQueryable<TDto> Transfer(IQueryable<TEntity> entities)
        {
            var query = Mapper.ProjectTo<TDto>(entities, Mapper.ConfigurationProvider);
            return query;
        }
    }
}
