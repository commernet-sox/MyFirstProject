using AutoMapper;
using Core.Database.Repository;
using Core.WebServices.Model;
using Core.WebServices.Service;
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
            var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            var query = from item in dbcontext.ConstructorInfo
                        select new ConstructorInfoDTO
                        {
                            Id = item.Id,
                            CompanyName = item.CompanyName,
                            Constructor = item.Constructor,
                            DateIssue = item.DateIssue,
                            PracticeSealNo = item.PracticeSealNo,
                            Province = item.Province,
                            QualificationCertNo = item.QualificationCertNo,
                            RegisterCertNo = item.RegisterCertNo,
                            RegisterMajor = item.RegisterMajor,
                            RegisterNumber = item.RegisterNumber,
                            ValidityRegistration = item.ValidityRegistration,
                            
                        };
            var result = base.PageDataWithQuery<ConstructorInfoDTO>(core_request, query);
            List<ConstructorInfoDTO> itemList = result.DtResponse.data as List<ConstructorInfoDTO>;
            result.DtResponse.data = itemList;
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
