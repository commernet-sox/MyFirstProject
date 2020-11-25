using CPC;
using System;
using System.Collections.Generic;

namespace Data.IdentityService
{
    public partial class SysMenuDTO : IMapDto
    {
        public string Mid { get; set; }
        public string Pid { get; set; }
        public string Mcode { get; set; }
        public string Mname { get; set; }
        public bool? IsLastLevel { get; set; }
        public string OpenName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string ParamList { get; set; }
        public int? Layer { get; set; }
        public int? Morder { get; set; }
        public string ShortCut { get; set; }
        public string SysCode { get; set; }
        public bool? IsSystem { get; set; }
        public string Memo { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool HasChildren
        {
            get
            {
                return !IsLastLevel.Value;
            }
        }
    }
}
