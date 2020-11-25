using Data.IdentityService;
using Data.IdentityService.Model;
using Infrastructure.IdentityService.Models;
using AutoMapper;
using CPC;
using CPC.DBCore;
using CPC.Redis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService
{
    public class SysUserService : BaseService<AMSContext, SysUser, SysUserDTO>
    {
        public SysUserService(IRepository<AMSContext, SysUser> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public Outcome Login(string userName, string password, bool isPda = false)
        {
            var user = Repository.Query.FirstOrDefault(t => t.UserId == userName);
            if (user == null)
            {
                return new Outcome(ApiCode.BadRequest, "用户名或密码错误");
            }
            password = DEncryptHelper.Encrypt(password);
            if (user.UserPwd != password && !isPda)
            {
                return new Outcome(ApiCode.BadRequest, "用户名或密码错误");
            }
            else if (user.Pdapwd != password && isPda)
            {
                return new Outcome(ApiCode.BadRequest, "用户名或密码错误");
            }

            if (!user.Status)
            {
                return new Outcome(ApiCode.InvalidData, "用户帐号处于非正常状态");
            }

            if (user.EmployeeId.IsNull())
            {
                return new Outcome(ApiCode.InvalidData, "未关联员工帐号");
            }

            if (user.ForbidLoginDate < DateTimeUtility.Now)
            {
                return new Outcome(ApiCode.InvalidData, "帐号已过期，禁止登录");
            }

            if (user.EnforceExpirePolicy == 1 && user.ExpireDate < DateTimeUtility.Now)
            {
                return new Outcome(ApiCode.InvalidData, "密码已过期，请先修改密码");
            }

            user.ForbidLoginDate = DateTimeUtility.Now.AddDays(30);
            Repository.Update(user);
            AddUserCache(user.UserId);
            return new Outcome();
        }

        private void AddUserCache(string userId)
        {
            using (var db = GlobalContext.Resolve<AMSContext>())
            {
                string sql = string.Format(@"select a.MID,a.MCode,a.MName,a.PID,a.OpenName,b.CommandCode,b.CommandName 
from sys_menu a left join sys_command b on a.mid=b.mid 
where a.sysCode='AMS'");
                var menus = db.FromSql(sql).Query<UserRoleMenuModel>().ToList();
                var redis = new RedisClient(Singleton<IConfiguration>.Instance["RedisConnection"]);
                redis.Set(userId + "_usercache", menus, TimeSpan.FromHours(2));
            }
        }

    }
}
