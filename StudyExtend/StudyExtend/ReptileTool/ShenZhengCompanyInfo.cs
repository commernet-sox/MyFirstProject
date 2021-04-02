using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyExtend.ReptileTool
{
    /// <summary>
    /// 建筑类企业资质证书信息 https://opendata.sz.gov.cn/data/dataSet/toDataDetails/29200_01903027
    /// </summary>
    public class ShenZhengCompanyInfo
    {
        public int page = 1;
        public string Url = "https://opendata.sz.gov.cn/api/1182425848/1/service.xhtml?page={0}&rows=10&appKey=2340b0b754f1408386ca20e1f0b34135";
        public int pageCount = 0;
        public void RequestUrl()
        {
            Url = string.Format(Url, page);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var result = HttpRequestMiddleware.SendRequest(Url, dic, HttpMethod.Get, new Dictionary<string, string>(), 3000);
            JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            if (pageCount == 0)
            {
                var totals = obj["total"].Value<int>();
                pageCount = totals % 10 > 0 ? totals / 10 + 1 : totals / 10;
            }
            var jArray = obj["data"].Value<JArray>();
            foreach (var item in jArray)
            {
                //var XH = item["XH"].ToString();
                //var FZJG = item["FZJG"].ToString();
                //var ZZZSH = item["ZZZSH"].ToString();
                //var FZRQ = item["FZRQ"].ToString();
                //var YXQ = item["YXQ"].ToString();
                //var ORG_CODE = item["ORG_CODE"].ToString();
                //var QYMC = item["QYMC"].ToString();
                //var QYYWLX = item["QYYWLX"].ToString();
                //var TYSHXYDM = item["TYSHXYDM"].ToString();
                //var ZZDJ = item["ZZDJ"].ToString();
                //var ZZLB = item["ZZLB"].ToString();
                //var ZZXL = item["ZZXL"].ToString();
                SZInfo sZInfo = new SZInfo();
                sZInfo= JsonConvert.DeserializeObject<SZInfo>(item.ToString());
            }
        }

    }
    public class SZInfo 
    {
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
