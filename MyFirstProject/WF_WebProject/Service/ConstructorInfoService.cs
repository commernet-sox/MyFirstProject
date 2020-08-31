using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Interface;
using WFWebProject.Models;

namespace WFWebProject.Service
{
    public class ConstructorInfoService : BaseService<ConstructorInfo, DataContext, ConstructorInfoDTO, int>,IConstructorInfoService
    {
        public ConstructorInfoService(IRepository<ConstructorInfo, DataContext> Repository, IMapper mapper) : base(Repository, mapper)
        {

        }
        protected override CoreResponse PageData(CoreRequest core_request)
        {
            //var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            //var query = from item in dbcontext.ConstructorInfo_new
            //            select new ConstructorInfoDTO
            //            {
            //                Id = item.Id,
            //                CompanyName = item.CompanyName,
            //                Constructor = item.Constructor,
            //                DateIssue = item.DateIssue,
            //                PracticeSealNo = item.PracticeSealNo,
            //                Province = item.Province,
            //                QualificationCertNo = item.QualificationCertNo,
            //                RegisterCertNo = item.RegisterCertNo,
            //                RegisterMajor = item.RegisterMajor,
            //                RegisterNumber = item.RegisterNumber,
            //                ValidityRegistration = item.ValidityRegistration,
            //                Remarks=item.Remarks,
            //            };
            var query = this.GetAll();
            var result = base.PageDataWithQuery<ConstructorInfoDTO>(core_request, query);
            List<ConstructorInfoDTO> itemList = result.DtResponse.data as List<ConstructorInfoDTO>;
            result.DtResponse.data = itemList;
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
                    ConstructorInfoDTO constructorInfoDTO = this.GetByID(Convert.ToInt32(Id));
                    if (constructorInfoDTO == null) continue;
                    if (data.ContainsKey("Remarks"))
                    {
                        constructorInfoDTO.Remarks = data["Remarks"].ToString();
                    }
                    constructorInfoDTO.ModifyTime = DateTime.Now;
                    constructorInfoDTO.Modifier = core_request.HttpContext.Session.GetString("User");
                    this.Update(constructorInfoDTO);
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
