using CPC;
using CPC.Service;
using SimpleWebApi.Core.Entities;
using SimpleWebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebApi.Application.Core
{
    public class BaseController : RestApiController
    {
        protected IOperate Operate { get; } = GlobalContext.Resolve<IOperate>();

        protected string UserId => Operate.UserId;

        protected T CreateEntity<T>()
    where T : BaseEntity, new() => Operate.CreateEntity<T>();

        protected T CreateDto<T>()
            where T : BaseDto, new() => Operate.CreateDto<T>();

        protected void Initialize(BaseEntity entity) => Operate.Initialize(entity);

        protected void Initialize(BaseDto dto) => Operate.Initialize(dto);
    }
}
