using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.IdentityService
{
    public class OAuth2CodeResponse
    {
        /// <summary>
        /// 授权码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 有效时间（秒）
        /// </summary>
        public int Expires { get; set; }
    }
}
