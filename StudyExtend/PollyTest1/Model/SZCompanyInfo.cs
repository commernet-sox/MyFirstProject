using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1.Model
{
    public class SZCompanyInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string XH { get; set; }
        public string FZJG { get; set; }
        public string ZZZSH { get; set; }
        public string FZRQ { get; set; }
        public string YXQ { get; set; }
        public string ORG_CODE { get; set; }
        public string QYMC { get; set; }
        public string QYYWLX { get; set; }
        public string TYSHXYDM { get; set; }
        public string ZZDJ { get; set; }
        public string ZZLB { get; set; }
        public string ZZXL { get; set; }
    }
}
