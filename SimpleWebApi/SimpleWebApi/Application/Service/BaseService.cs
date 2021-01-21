using AutoMapper;
using CPC;
using CPC.DBCore;
using SimpleWebApi.Application.Core;
using SimpleWebApi.Core.Entities;
using SimpleWebApi.Data;
using SimpleWebApi.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Service
{
    public class BaseService<TEntity, TDto> : BaseService<SimpleWebApiContext, TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseDto
    {
        private readonly IOperate _operate;

        public BaseService(IRepository<SimpleWebApiContext, TEntity> repository, IMapper mapper, IOperate operate) : base(repository, mapper)
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
