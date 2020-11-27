using CPC;
using CPC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Entities;
using TestData;

namespace TestApi.Application.Core
{
    public class BaseController: RestApiController
    {
        protected IOperate Operate { get; } = GlobalContext.Resolve<IOperate>();

        protected string UserId => Operate.UserId;

        protected T CreateEntity<T>()
    where T : BaseEntity, new() => Operate.CreateEntity<T>();

        protected T CreateDto<T>()
            where T : BaseDTO, new() => Operate.CreateDto<T>();

        protected void Initialize(BaseEntity entity) => Operate.Initialize(entity);

        protected void Initialize(BaseDTO dto) => Operate.Initialize(dto);
    }
}
