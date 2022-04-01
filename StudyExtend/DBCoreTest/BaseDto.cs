using CPC;
using CPC.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBCoreTest
{
    [MapperIgnore]
    public class BaseDto : IMapDto
    {
        /// <summary>
        /// 主键自增ID
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual string ModifyBy { get; set; }

        /// <summary>
        /// 是否已被删除
        /// </summary>
        public virtual bool IsDeleted { get; set; } = false;
    }
}
