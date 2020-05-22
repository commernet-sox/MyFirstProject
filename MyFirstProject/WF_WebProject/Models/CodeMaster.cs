using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.Models
{
    public class CodeMaster
    {
        public int Id { get; set; }
        public Nullable<DateTime> ModifyTime { get; set; }
        public string Modifier { get; set; }
        public DateTime CreateTime { get; set; }
        public string Creator { get; set; }
        public string CodeGroup { get; set; }
        public string CodeId { get; set; }
        public string CodeName { get; set; }
        public char IsActive { get; set; }
        public string Remarks { get; set; }
        public string HUDF_01 { get; set; }
        

    }
}
