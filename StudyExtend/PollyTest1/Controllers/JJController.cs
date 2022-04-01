using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PollyTest1.Model;
using StudyExtend.ReptileTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JJInfo = PollyTest1.Model.jjInfo;

namespace PollyTest1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JJController : ControllerBase
    {
        private readonly PollyTestDbContext _dbContext;
        public JJController(PollyTestDbContext pollyTestDbContext)
        {
            _dbContext = pollyTestDbContext;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public string Add()
        //{
        //    int page = 1;
        //    string Url = "http://api.fund.eastmoney.com/f10/lsjz?callback=jQuery183011157682279008041_1631951383257&fundCode=161725&pageIndex={1}&pageSize=20&startDate=&endDate=&_={0}";
        //    int pageCount = 78;
        //    var timeNow = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("Cookie", "EMFUND1=null; EMFUND2=null; EMFUND3=null; EMFUND4=null; EMFUND5=null; EMFUND6=null; EMFUND7=null; EMFUND8=null; EMFUND0=null; qgqp_b_id=f34c3956048afd7c504e9b67ceda6879; st_si=89698550515158; st_asi=delete; EMFUND9=09-18 15:48:54@#$%u62DB%u5546%u4E2D%u8BC1%u767D%u9152%u6307%u6570%28LOF%29A@%23%24161725; st_pvi=60455906660920; st_sp=2021-09-18%2014%3A57%3A07; st_inirUrl=http%3A%2F%2Ffund.eastmoney.com%2Ffund.html; st_sn=6; st_psi=20210918154943227-112200305283-9015325277");
        //    dic.Add("Host", "api.fund.eastmoney.com");
        //    dic.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
        //    dic.Add("Referer", "http://fundf10.eastmoney.com/");
        //    //var result = HttpRequestMiddleware.SendRequest(Url, new Dictionary<string, string>(), HttpMethod.Get, dic, 3000);
        //    //result = Regex.Replace(result, @".*^\(", @"(");
        //    //JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
        //    if (pageCount != 0)
        //    {
        //        for (int i = 1; i < pageCount; i++)
        //        {
        //            string result1 = "";
        //            System.Threading.Thread.Sleep(1000);
        //            try
        //            {
        //                timeNow = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        //                result1 = HttpRequestMiddleware.SendRequest(string.Format(Url, timeNow, i), new Dictionary<string, string>(), HttpMethod.Get, dic, 30000);
        //                result1 = Regex.Replace(result1, @".*\(", @"(");
        //                result1 = result1.Substring(1,result1.Length-2);
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Threading.Thread.Sleep(10000);
        //                i--;
        //                continue;
        //            }

        //            JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result1);
        //            if (pageCount == 0)
        //            {
        //                var totals = obj["TotalCount"].Value<int>();
        //                pageCount = totals % 20 > 0 ? totals / 20 + 1 : totals / 20;
        //            }
        //            var jobject = obj["Data"].Value<JObject>();
        //            var jArray = jobject["LSJZList"].Value<JArray>();

        //            foreach (var item in jArray)
        //            {
        //                JJInfo jjInfo = new JJInfo();
        //                jjInfo = JsonConvert.DeserializeObject<JJInfo>(item.ToString());
        //                _dbContext.Add(jjInfo);
        //            }
        //        }
        //    }
        //    _dbContext.SaveChanges();

        //    return "Success";
        //}
    }
}
