using Core.WebServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WFWebProject.DTO
{
    public class MenuInfoDTO:BaseDTO
    {
        public int Id { get; set; }
        public string TitleId { get; set; }
        public string Title { get; set; }
        public string ContentId { get; set; }
        public string Content { get; set; }
    }
}
