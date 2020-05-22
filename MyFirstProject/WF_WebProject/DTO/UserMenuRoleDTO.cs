using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class UserMenuRoleDTO:BaseDTO
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        [AutoMapper.IgnoreMap]
        public IEnumerable<MenuInfoDTO> Contents { get; set; }
        [AutoMapper.IgnoreMap]
        public string UserName { get; set; }
    }
}
