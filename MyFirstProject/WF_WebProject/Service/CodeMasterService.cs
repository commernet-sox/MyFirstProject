using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Interface;
using Core.WebServices.Model;
using Core.WebServices.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Interface;
using WFWebProject.Models;
using Core.Infrastructure.DataTables;
using Microsoft.AspNetCore.Http;

namespace WFWebProject.Service
{
    public class CodeMasterService : BaseService<CodeMaster, DataContext, CodeMasterDTO, int>,ICodeMasterService
    {
        public CodeMasterService(IRepository<CodeMaster, DataContext> Repository, IMapper mapper) : base(Repository, mapper)
        {
            
        }
        protected override CoreResponse PageData(CoreRequest core_request)
        {
            var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            var query = from ware in dbcontext.CodeMaster
                        select new CodeMasterDTO
                        {
                            Id = ware.Id,
                            ModifyTime = ware.ModifyTime,
                            Modifier = ware.Modifier,
                            CreateTime = ware.CreateTime,
                            Creator = ware.Creator,
                            CodeGroup = ware.CodeGroup,
                            CodeId = ware.CodeId,
                            CodeName = ware.CodeName,
                            IsActive = ware.IsActive,
                            Remarks = ware.Remarks,
                            HUDF_01 = ware.HUDF_01,
                        };
            var result = base.PageDataWithQuery<CodeMasterDTO>(core_request, query);
            List<CodeMasterDTO> warehouseBasicList = result.DtResponse.data as List<CodeMasterDTO>;
            
            result.DtResponse.data = warehouseBasicList;
            return result;
        }

        public CoreResponse EditData(string Id, CoreRequest core_request)
        {
            try
            {
                var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
                CoreResponse core_response = new CoreResponse(core_request);
                foreach (var item in core_request.DtRequest.Data)
                {
                    string key = item.Key;
                    var data = item.Value as Dictionary<string, object>;
                    CodeMasterDTO codeMasterDTO = this.GetByID(Convert.ToInt32(Id));
                    if (codeMasterDTO == null) continue;
                    if (data.ContainsKey("Remarks"))
                    {
                        codeMasterDTO.Remarks = data["Remarks"].ToString();
                    }
                    codeMasterDTO.ModifyTime = DateTime.Now;
                    codeMasterDTO.Modifier = core_request.HttpContext.Session.GetString("User");
                    this.Update(codeMasterDTO);
                }
                return core_response;
            }
            catch (Exception ex)
            {
                return new CoreResponse(core_request);
            }
        }
        protected override CoreResponse Create(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Edit(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Remove(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }

        protected override CoreResponse Upload(CoreRequest core_request)
        {
            throw new NotImplementedException();
        }
    }
}
