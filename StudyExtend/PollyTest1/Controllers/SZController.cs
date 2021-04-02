using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PollyTest1.Model;
using StudyExtend.ReptileTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyTest1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SZController : ControllerBase
    {
        private readonly PollyTestDbContext _dbContext;
        public SZController(PollyTestDbContext pollyTestDbContext)
        {
            _dbContext = pollyTestDbContext;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Add()
        {
            int page = 437;
            string Url = "https://opendata.sz.gov.cn/api/1182425848/1/service.xhtml?page={0}&rows=100&appKey=2340b0b754f1408386ca20e1f0b34135";
            int pageCount = 0;
            if (pageCount == 0)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var result = HttpRequestMiddleware.SendRequest(string.Format(Url, page), dic, HttpMethod.Get, new Dictionary<string, string>(), 3000);
                JObject obj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                if (pageCount == 0)
                {
                    var totals = obj["total"].Value<int>();
                    pageCount = totals % 10 > 0 ? totals / 10 + 1 : totals / 10;
                }
                var jArray = obj["data"].Value<JArray>();
                foreach (var item in jArray)
                {
                    SZCompanyInfo sZInfo = new SZCompanyInfo();
                    sZInfo = JsonConvert.DeserializeObject<SZCompanyInfo>(item.ToString());
                    var res= _dbContext.Add(sZInfo);
                    
                }
                //if (pageCount != 0)
                //{
                //    for (int i = 2; i < pageCount; i++)
                //    {
                //        string result1 = "";
                //        System.Threading.Thread.Sleep(1000);
                //        try
                //        {
                //            result1 = HttpRequestMiddleware.SendRequest(string.Format(Url, i), dic, HttpMethod.Get, new Dictionary<string, string>(), 30000);
                //        }
                //        catch (Exception ex)
                //        {
                //            System.Threading.Thread.Sleep(10000);
                //            i--;
                //            continue;
                //        }

                //        JObject obj1 = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result1);
                //        if (pageCount == 0)
                //        {
                //            var totals = obj1["total"].Value<int>();
                //            pageCount = totals % 10 > 0 ? totals / 10 + 1 : totals / 10;
                //        }
                //        var jArray1 = obj1["data"].Value<JArray>();
                //        foreach (var item in jArray1)
                //        {
                //            SZCompanyInfo sZInfo = new SZCompanyInfo();
                //            sZInfo = JsonConvert.DeserializeObject<SZCompanyInfo>(item.ToString());
                //            _dbContext.SZCompanyInfo.Add(sZInfo);
                //        }
                //    }
                //}
                _dbContext.SaveChanges();
            }

            return "Success";
        }
    }
}
