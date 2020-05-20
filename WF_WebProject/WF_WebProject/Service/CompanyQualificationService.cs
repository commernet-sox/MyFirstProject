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
    public class CompanyQualificationService : BaseService<CompanyQualification, DataContext, CompanyQualificationDTO, int>, ICompanyQualificationService
    {
        public CompanyQualificationService(IRepository<CompanyQualification, DataContext> Repository, IMapper mapper) : base(Repository, mapper)
        {

        }
        protected override CoreResponse PageData(CoreRequest core_request)
        {
            var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            var query = from item in dbcontext.CompanyQualification
                        select new CompanyQualificationDTO
                        {
                            Id = item.Id,
                            City = item.City,
                            Code = item.Code,
                            ComprehensiveScore = item.ComprehensiveScore,
                            ContactAddress = item.ContactAddress,
                            EconomicType = item.EconomicType,
                            Email = item.Email,
                            EndDate = item.EndDate,
                            IssuingAuthority = item.IssuingAuthority,
                            Name = item.Name,
                            OrganizationCode = item.OrganizationCode,
                            Province = item.Province,
                            QualificationType = item.QualificationType,
                            SafetyLicenseNo = item.SafetyLicenseNo,
                            ScopeLicense = item.ScopeLicense,
                            StartDate = item.StartDate,
                            Time = item.Time,
                            WebSite = item.WebSite,
                            ZipCode = item.ZipCode,
                            

                        };
            var result = base.PageDataWithQuery<CompanyQualificationDTO>(core_request, query);
            List<CompanyQualificationDTO> itemList = result.DtResponse.data as List<CompanyQualificationDTO>;
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
