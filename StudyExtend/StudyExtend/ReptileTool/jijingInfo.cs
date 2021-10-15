using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StudyExtend.ReptileTool
{
    public class jijingInfo
    {
        public static int page = 1;
        public static string Url = "http://api.fund.eastmoney.com/f10/lsjz?callback=jQuery183011157682279008041_1631951383257&fundCode=161725&pageIndex={1}&pageSize=20&startDate=&endDate=&_={0}";
        public static int pageCount = 0;
        public static void RequestUrl()
        {
            var timeNow = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            Url = string.Format(Url, timeNow, page);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("Cookie", "EMFUND1=null; EMFUND2=null; EMFUND3=null; EMFUND4=null; EMFUND5=null; EMFUND6=null; EMFUND7=null; EMFUND8=null; EMFUND0=null; qgqp_b_id=f34c3956048afd7c504e9b67ceda6879; st_si=89698550515158; st_asi=delete; EMFUND9=09-18 15:48:54@#$%u62DB%u5546%u4E2D%u8BC1%u767D%u9152%u6307%u6570%28LOF%29A@%23%24161725; st_pvi=60455906660920; st_sp=2021-09-18%2014%3A57%3A07; st_inirUrl=http%3A%2F%2Ffund.eastmoney.com%2Ffund.html; st_sn=6; st_psi=20210918154943227-112200305283-9015325277");
            dic.Add("Host", "api.fund.eastmoney.com");
            dic.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            dic.Add("Referer", "http://fundf10.eastmoney.com/");
            var result = HttpRequestMiddleware.SendRequest(Url, new Dictionary<string, string>(), HttpMethod.Get, dic, 3000);
            result = Regex.Replace(result,@".*^\(",@"(");
            JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            if (pageCount == 0)
            {
                var totals = obj["TotalCount"].Value<int>();
                pageCount = totals % 20 > 0 ? totals / 20 + 1 : totals / 20;
            }
            var jobject = obj["data"].Value<JObject>();
            var jArray = jobject["LSJZList"].Value<JArray>();

            foreach (var item in jArray)
            {
                JJInfo jjInfo = new JJInfo();
                jjInfo = JsonConvert.DeserializeObject<JJInfo>(item.ToString());

            }
        }


    }

    public class JJInfo
    {
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
