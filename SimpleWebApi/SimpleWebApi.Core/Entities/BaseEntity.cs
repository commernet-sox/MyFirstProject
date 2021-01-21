using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CPC;

namespace SimpleWebApi.Core.Entities
{
    public class BaseEntity:IMapEntity
    {
        /// <summary>
        /// 主键自增ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(30), Required]
        public virtual string CreateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [MaxLength(30)]
        public virtual string ModifyBy { get; set; }

        /// <summary>
        /// 是否数据已删除
        /// </summary>
        public virtual bool IsDeleted { get; set; } = false;
    }
}
