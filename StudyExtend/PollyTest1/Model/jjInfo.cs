using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1.Model
{
    public class jjInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FSRQ { get; set; }
        public string DWJZ { get; set; }
        public string LJJZ { get; set; }
        public string SDATE { get; set; }
        public string ACTUALSYI { get; set; }
        public string NAVTYPE { get; set; }
        public string JZZZL { get; set; }
        public string SGZT { get; set; }
        public string SHZT { get; set; }
        public string FHFCZ { get; set; }
        public string FHFCBZ { get; set; }
        public string DTYPE { get; set; }
        public string FHSP { get; set; }
    }
}
