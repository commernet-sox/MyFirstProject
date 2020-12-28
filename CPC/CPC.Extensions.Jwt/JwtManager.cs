using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CPC.Extensions
{
    public class JwtManager
    {
        #region Members
        private readonly JwtSettings _jwtSettings;
        #endregion

        #region Constructors
        public JwtManager(JwtSettings jwtSettings) => _jwtSettings = jwtSettings;
        #endregion

        #region Methods
        public JwtToken CreateToken(TimeSpan exp, string grantType = JwtGrantType.ClientCredential, string scope = JwtScopes.Scope, List<Claim> subject = null)
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var issTime = DateTime.UtcNow;
            var expiresAt = issTime.Add(exp);//过期时间

            var claims = new List<Claim>()
            {
                new Claim(JwtCustomNames.GrantType,grantType),
                new Claim(JwtRegisteredClaimNames.Jti,RandomUtility.GuidString()),
                new Claim(JwtCustomNames.Scope,scope)
            };

            if (!subject.IsNull())
            {
                foreach (var c in subject)
                {
                    claims.Add(c);
                }
            }

            var tokenDesc = new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Issuer,//签发人
                Expires = expiresAt,//过期时间
                Subject = new ClaimsIdentity(claims),
                Audience = _jwtSettings.Audience,//受众
                NotBefore = issTime,//生效时间
                IssuedAt = issTime,//签发时间

                //签名证书
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);
            var tokenString = tokenHandler.WriteToken(token);

            var respone = new JwtToken
            {
                Token = tokenString,
                Expires = exp.TotalSeconds.ConvertInt32(),
                GrantType = grantType,
                Scope = scope
            };

            return respone;
        }

        public TokenValidationParameters ValidationParameters(JwtSettings jwtSettings, Action<TokenValidationParameters> jwtValidationSetup = null)
        {
            var param = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                RequireSignedTokens = true,
                SaveSigninToken = true,
                // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                // 是否要求Token的Claims中必须包含Expires
                RequireExpirationTime = true,
                // 允许的服务器时间偏移量
                ClockSkew = TimeSpan.FromSeconds(30),
                // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                ValidateLifetime = true
            };

            jwtValidationSetup?.Invoke(param);

            return param;
        }

        public Outcome<List<Claim>> ValidateToken(string token, Action<TokenValidationParameters> jwtValidationSetup = null) => ValidateToken(token, jwtValidationSetup, out var _);

        public Outcome<List<Claim>> ValidateToken(string token, Action<TokenValidationParameters> jwtValidationSetup, out ClaimsPrincipal principal)
        {
            principal = null;
            var oc = new Outcome<List<Claim>>();

            if (token.IsNull())
            {
                oc.Code = ApiCode.DataMissing;
                oc.Message = "access token missing";
                return oc;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
            {
                oc.Code = ApiCode.InvalidData;
                oc.Message = "incorrect access token format";
                return oc;
            }

            var param = ValidationParameters(_jwtSettings, jwtValidationSetup);
            try
            {
                principal = tokenHandler.ValidateToken(token, param, out _);
                oc.Data = principal.Claims.ToList();
                oc.Message = "success";
            }
            catch (SecurityTokenExpiredException)
            {
                oc.Code = ApiCode.DataExpired;
                oc.Message = "access token has expired";
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                oc.Code = ApiCode.InvalidData;
                oc.Message = "access token resolution failed";
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                oc.Code = ApiCode.InvalidData;
                oc.Message = "invalid access token";
            }
            catch (ArgumentException)
            {
                oc.Code = ApiCode.InvalidData;
                oc.Message = "incorrect access token format";
            }
            catch (Exception)
            {
                oc.Code = ApiCode.InternalServerError;
                oc.Message = "access token verification failed";
            }
            return oc;
        }
        #endregion
    }
}
