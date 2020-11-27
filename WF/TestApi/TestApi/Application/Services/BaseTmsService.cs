using AutoMapper;
using CPC;
using CPC.DBCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApi.Application.Core;
using TestCore.Entities;
using TestData;
using TestInfrastructure;

namespace TestApi.Application.Services
{
    public class BaseTmsService<TEntity, TDto> : BaseService<TestContext, TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDTO
    {
        private readonly IOperate _operate;

        public BaseTmsService(IRepository<TestContext, TEntity> repository, IMapper mapper, IOperate operate) : base(repository, mapper)
        {
            _operate = operate;
        }

        protected override TEntity Transfer(TDto dto)
        {
            var entity = base.Transfer(dto);
            if (entity.CreateBy.IsNull() || entity.CreateTime == default)
            {
                entity.CreateBy = _operate.UserId;
                if (entity.CreateTime != default)
                {
                }
                else
                {
                    entity.CreateTime = DateTimeUtility.Now;
                }
            }
            else
            {
                entity.ModifyBy = _operate.UserId;
                entity.ModifyTime = DateTimeUtility.Now;
            }
            return entity;
        }
    }
}
