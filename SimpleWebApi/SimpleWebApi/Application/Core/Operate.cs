using CPC;
using Microsoft.AspNetCore.Http;
using SimpleWebApi.Core.Entities;
using SimpleWebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Core
{
    public class Operate : IOperate
    {
        private readonly IHttpContextAccessor _accessor;

        public Operate(IHttpContextAccessor accessor) => _accessor = accessor;

        public string UserId
        {
            get
            {
                if (_accessor.HttpContext.Request.Headers.ContainsKey("UserId"))
                {
                    return _accessor.HttpContext.Request.Headers["UserId"];
                }
                return "07508";
            }
        }

        public T CreateEntity<T>()
            where T : BaseEntity, new()
        {
            var result = new T()
            {
                CreateBy = UserId,
                CreateTime = DateTimeUtility.Now
            };
            return result;
        }

        public T CreateDto<T>()
            where T : BaseDto, new()
        {
            var result = new T()
            {
                CreateBy = UserId,
                CreateTime = DateTimeUtility.Now
            };
            return result;
        }

        public void Initialize(BaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity.CreateBy.IsNull() || entity.CreateTime == default)
            {
                entity.CreateBy = UserId;
                if (entity.CreateTime != default)
                {
                }
                else
                {
                    entity.CreateTime = DateTimeUtility.Now;
                }
            }

            entity.ModifyBy = UserId;
            entity.ModifyTime = DateTimeUtility.Now;
        }

        public void Initialize(BaseDto dto)
        {
            if (dto == null)
            {
                return;
            }

            if (dto.CreateBy.IsNull() || dto.CreateTime == default)
            {
                dto.CreateBy = UserId;
                if (dto.CreateTime != default)
                {
                }
                else
                {
                    dto.CreateTime = DateTimeUtility.Now;
                }
            }

            dto.ModifyBy = UserId;
            dto.ModifyTime = DateTimeUtility.Now;
        }
    }

    public interface IOperate
    {
        string UserId { get; }

        T CreateEntity<T>()
            where T : BaseEntity, new();

        T CreateDto<T>()
            where T : BaseDto, new();

        void Initialize(BaseEntity entity);

        void Initialize(BaseDto dto);
    }
}
