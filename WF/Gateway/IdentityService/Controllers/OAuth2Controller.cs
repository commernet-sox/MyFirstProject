using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CPC;
using CPC.Redis;
using CPC.Service;
using IdentityService.RequestEntities;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class OAuth2Controller : RestApiController
    {
        public OAuth2Controller()
        {

        }
        /// <summary>
        /// 获取临时凭证（access_token）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<OAuth2GetResponse> Get([FromServices] JwtManager jwtManager, [FromQuery] OAuth2GetRequest request)
        {
            if (!request.GrantType.In(JwtGrantType.ClientCredential, JwtGrantType.PasswordCredential, JwtGrantType.RefreshToken, JwtGrantType.AuthorizationCode))
            {
                return Custom(ApiCode.NotOpened, "暂不支持该授权类型");
            }

            //using (var db = GlobalContext.Resolve<AMSContext>())
            //{
            //    var app = db.AuthApp.FirstOrDefault(t => t.AppKey == request.ClientId && t.AppSecret == request.ClientSecret);

            //    if (app == null)
            //    {
            //        return Custom(ApiCode.InvalidData, "client信息不正确");
            //    }

            //    if (app.Status.GetValueOrDefault() == false)
            //    {
            //        return Custom(ApiCode.AccessLimit, "帐号处于非正常状态");
            //    }
            //}

            var refreshToken = string.Empty;

            switch (request.GrantType)
            {
                //case JwtGrantType.PasswordCredential:
                //    {
                //        if (request.UserName.IsNull() || request.Password.IsNull())
                //        {
                //            return Custom(ApiCode.DataMissing, "用户名和密码不能为空");
                //        }
                //        using var user = GlobalContext.Resolve<SysUserService>();
                //        var oc = user.Login(request.UserName, request.Password);
                //        if (oc.Code != ApiCode.Success)
                //        {
                //            return Custom(oc);
                //        }

                //        request.UserName = request.UserName.ToUpperInvariant();
                //        refreshToken = RandomUtility.GuidString();
                //        break;
                //    }

                case JwtGrantType.AuthorizationCode:
                    {
                        if (request.Code.IsNull())
                        {
                            return Custom(ApiCode.DataMissing, "授权码不能为空");
                        }

                        var redis = GlobalContext.Resolve<RedisClient>();
                        var user = redis.String.Get<string>("ac_" + request.Code);
                        if (user.IsNull())
                        {
                            return Custom(ApiCode.InvalidData, "authorization code无效或者已过期");
                        }

                        redis.Key.Del("ac_" + request.Code);
                        refreshToken = RandomUtility.GuidString();
                        request.UserName = user;
                        break;
                    }

                //case JwtGrantType.RefreshToken:
                //    {
                //        if (request.RefreshToken.IsNull())
                //        {
                //            return Custom(ApiCode.DataMissing, "refresh token不能为空");
                //        }

                //        var redis = GlobalContext.Resolve<RedisClient>();

                //        var body = redis.String.Get<RefreshTokenBody>("rt_" + request.RefreshToken);
                //        if (body == null)
                //        {
                //            return Custom(ApiCode.InvalidData, "refresh token无效或者已过期");
                //        }

                //        if (body.ClientId != request.ClientId)
                //        {
                //            return Custom(ApiCode.InvalidData, "refresh token不正确");
                //        }

                //        refreshToken = RandomUtility.GuidString();
                //        request.UserName = body.UserName;
                //        redis.Key.Del("rt_" + request.RefreshToken);
                //        break;
                //    }
                default:
                    request.UserName = string.Empty;
                    break;
            }

            //if (!refreshToken.IsNull())
            //{
            //    var redis = GlobalContext.Resolve<RedisClient>();
            //    redis.String.Set("rt_" + refreshToken, new RefreshTokenBody { ClientId = request.ClientId, UserName = request.UserName, Scope = request.Scope }, TimeSpan.FromDays(30));
            //}

            var claims = new List<Claim>() { new Claim("ClientId", request.ClientId), new Claim("Scope", "scope") };
            if (!request.UserName.IsNull())
            {
                claims.Add(new Claim("UserId", request.UserName.ToLowerInvariant()));
            }

            var token = jwtManager.CreateToken(TimeSpan.FromHours(2), request.GrantType, JwtScopes.Scope, claims);

            return new OAuth2GetResponse
            {
                Expires = token.Expires,
                Scope = JwtScopes.Scope,
                Token = token.Token,
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// 获取登录用户授权码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpGet("code")]
        public ActionResult<OAuth2CodeResponse> GetCode(string userId)
        {
            var code = RandomUtility.GuidString();
            var redis = GlobalContext.Resolve<RedisClient>();
            var ttl = TimeSpan.FromHours(2);
            redis.Set("ac_" + code, userId.ToUpperInvariant(), ttl);

            return new OAuth2CodeResponse { Code = code, Expires = ttl.TotalSeconds.ConvertInt32() };
        }
    }
}
