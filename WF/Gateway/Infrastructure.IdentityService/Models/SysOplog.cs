using CPC;
using System;
using System.Collections.Generic;

namespace Infrastructure.IdentityService.Models
{
    public partial class SysOplog : IMapEntity
    {
        public long LogId { get; set; }
        public int? LogType { get; set; }
        public string SysCode { get; set; }
        public string Mid { get; set; }
        public string Mname { get; set; }
        public string CommandId { get; set; }
        public string CommandName { get; set; }
        public string KeyId { get; set; }
        public string TableName { get; set; }
        public string OperationEmp { get; set; }
        public DateTime? OperationDate { get; set; }
        public string ClientName { get; set; }
        public string Ip { get; set; }
        public string Memo { get; set; }
        public int? Ex1 { get; set; }
        public string Ex2 { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
