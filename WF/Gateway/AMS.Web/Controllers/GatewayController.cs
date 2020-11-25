using Data.IdentityService.Model;
using AMS.WebCore;
using CPC;
using CPC.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Web.Controllers
{
    public class GatewayController: BaseController
    {
        public IActionResult Gateway()
        {
            return View();
        }
        public PageData<List<GatewayModel>> GetList()
        {
            var res = Get("/admin/configuration").AsRawJsonObject();
            var result = res.Result;
            JArray array = (JArray)result["routes"];
            List<GatewayModel> gateways = new List<GatewayModel>();
            foreach (var item in array)
            {
                GatewayModel gatewayModel = new GatewayModel();
                gatewayModel.ServiceName = ((JObject)item)["serviceName"].ToString();
                gatewayModel.DownstreamPathTemplate= ((JObject)item)["downstreamPathTemplate"].ToString();
                gatewayModel.DownstreamScheme = ((JObject)item)["downstreamScheme"].ToString();
                gatewayModel.UpstreamPathTemplate = ((JObject)item)["upstreamPathTemplate"].ToString();
                gateways.Add(gatewayModel);
            }
            //var result = Get("/admin/configuration").As<PageData<object>>().Result;
            return new PageData<List<GatewayModel>>(0,0, gateways.Count(), gateways);
        }

        public Outcome Insert(GatewayModel dto)
        {
            //var result = Post("/os/1.0/gateway/insert", dto).As<Outcome>().Result;
            GatewayModel gatewayModel = new GatewayModel();
            gatewayModel.ServiceName = dto.ServiceName;
            gatewayModel.DownstreamPathTemplate = dto.DownstreamPathTemplate;
            gatewayModel.DownstreamScheme = dto.DownstreamScheme;
            gatewayModel.UpstreamPathTemplate = dto.UpstreamPathTemplate;

            var res = Get("/admin/configuration").AsRawJsonObject();
            var result = res.Result;
            JArray array = (JArray)result["routes"];
            //判断是否重复添加
            foreach (var item in array)
            {
                if (((JObject)item)["serviceName"].ToString() == gatewayModel.ServiceName || ((JObject)item)["upstreamPathTemplate"].ToString() == gatewayModel.UpstreamPathTemplate)
                {
                    return new Outcome(ApiCode.DataLimit,"已存在相同名称的信息，请检查后再试！");
                }
            }
            array.Add(JObject.FromObject(gatewayModel));
            res.Result["routes"] = array;
            var newGateway = res.Result.ToString();
            var rut = Post("/admin/configuration", new StringContent(newGateway, Encoding.UTF8,"application/json"));
            return new Outcome(ApiCode.Success, rut.IsSuccessStatusCode == true ? "成功" : "失败");
        }

        public Outcome Update(GatewayModel dto,string id)
        {
            GatewayModel gatewayModel = new GatewayModel();
            gatewayModel.ServiceName = dto.ServiceName;
            gatewayModel.DownstreamPathTemplate = dto.DownstreamPathTemplate;
            gatewayModel.DownstreamScheme = dto.DownstreamScheme;
            gatewayModel.UpstreamPathTemplate = dto.UpstreamPathTemplate;

            var res = Get("/admin/configuration").AsRawJsonObject();
            var result = res.Result;
            JArray array = (JArray)result["routes"];
            //删除原始的数据
            JToken origin=null;
            foreach (var item in array)
            {
                if (((JObject)item)["serviceName"].ToString() == id)
                {
                    origin = item;
                    continue;
                }
                if (((JObject)item)["serviceName"].ToString() == gatewayModel.ServiceName || ((JObject)item)["upstreamPathTemplate"].ToString() == gatewayModel.UpstreamPathTemplate)
                {
                    return new Outcome(ApiCode.DataLimit, "已存在相同名称的信息，请检查后再试！");
                }
            }
            if (!origin.IsNull())
            {
                array.Remove(origin);
            }
            array.Add(JObject.FromObject(gatewayModel));
            res.Result["routes"] = array;
            var newGateway = res.Result.ToString();
            var rut = Post("/admin/configuration", new StringContent(newGateway, Encoding.UTF8, "application/json"));
            return new Outcome(ApiCode.Success, rut.IsSuccessStatusCode == true ? "成功" : "失败");
        }

        public Outcome Delete(string id)
        {
            var res = Get("/admin/configuration").AsRawJsonObject();
            var result = res.Result;
            JArray array = (JArray)result["routes"];
            //删除原始的数据
            JToken origin = null;
            foreach (var item in array)
            {
                if (((JObject)item)["serviceName"].ToString() == id)
                {
                    origin = item;
                    break;
                }
            }
            if (!origin.IsNull())
            {
                array.Remove(origin);
            }
            res.Result["routes"] = array;
            var newGateway = res.Result.ToString();
            var rut = Post("/admin/configuration", new StringContent(newGateway, Encoding.UTF8, "application/json"));
            return new Outcome(rut.IsSuccessStatusCode==true?ApiCode.Success:ApiCode.AccessLimit, rut.IsSuccessStatusCode == true ? "成功" : "失败");
        }
    }
}
