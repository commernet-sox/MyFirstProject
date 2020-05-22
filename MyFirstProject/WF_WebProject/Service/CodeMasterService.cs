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
