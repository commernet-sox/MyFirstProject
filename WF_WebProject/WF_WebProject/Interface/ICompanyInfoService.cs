using Core.WebServices.Interface;
using Core.WebServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Models;

namespace WFWebProject.Interface
{
    public interface ICompanyInfoService : IBase<CompanyInfo, CompanyInfoDTO, int>, IDatatable
    {
        CoreResponse CompanyAllPageData(CoreRequest core_request);
    }
}
