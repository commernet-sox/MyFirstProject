using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.IdentityService;
using Data.IdentityService.Model;
using Infrastructure.IdentityService.Models;
using CPC;
using CPC.Redis;
using CPC.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IdentityService
{
    public class BaseController : RestApiController
    {
        #region 记录日志
        protected virtual void WriteLog(int logType, string mid, string mname, string commandId, string commmandName, string keyId, string tableName, string memo = "")
        {
            SysOplog entity = new SysOplog
            {
                LogType = logType,
                SysCode = "AMS",
                Mid = mid,
                Mname = mname,
                CommandId = commandId,
                CommandName = commmandName,
                KeyId = keyId,
                TableName = tableName,
                OperationEmp = UserId,
                OperationDate = DateTime.Now,
                Memo = memo,
                CreateBy = UserId,
                CreateDate = DateTime.Now,
                ClientName = Request.Host.Host,
                Ip = Request.Host.Host
            };
            WriteLog(entity);
        }

        protected virtual void WriteLog(SysOplog entity)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                db.SysOplog.Add(entity);
                db.SaveChanges();
            }
        }
        #endregion

        #region Cache
        protected virtual T GetCache<T>(string key)
        {
            var redis = new RedisClient(Singleton<IConfiguration>.Instance["RedisConnection"]);
            return redis.Get<T>(key);
        }
        #endregion

        #region Common
        protected string UserId
        {
            get
            {
                if (Request.Headers.ContainsKey("UserId"))
                {
                    return Request.Headers["UserId"];
                }
                return string.Empty;
            }
        }

        protected virtual UserRoleMenuModel GetMenuInfo(string controllerName)
        {
            string key = UserId + "_usercache";
            var menus = GetCache<List<UserRoleMenuModel>>(key);
            if (menus != null)
            {
                var menu = menus.FirstOrDefault(item => item.MCode == controllerName);
                return menu;
            }
            return null;
        }
        #endregion

    }
}