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
    public interface ICodeMasterService : IBase<CodeMaster, CodeMasterDTO, int>, IDatatable
    {
        CoreResponse EditData(string Id, CoreRequest core_request);
    }
}
