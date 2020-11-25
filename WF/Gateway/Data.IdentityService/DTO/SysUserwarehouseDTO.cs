using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class SysUserwarehouseDTO : IMapDto
    {
        public string UserId { get; set; }
        public string WarehouseId { get; set; }
        public string CommandId { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Ex1 { get; set; }
        public string Ex2 { get; set; }
        public int? Ex3 { get; set; }
        public int? Ex4 { get; set; }
    }
}
