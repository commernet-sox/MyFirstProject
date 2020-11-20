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
    public class CompanyInfoService : BaseService<CompanyInfo, DataContext, CompanyInfoDTO, int>, ICompanyInfoService
    {
        private ICompanyQualificationService _companyQualificationService;
        public CompanyInfoService(IRepository<CompanyInfo, DataContext> Repository, IMapper mapper,ICompanyQualificationService companyQualificationService) : base(Repository, mapper)
        {
            _companyQualificationService=companyQualificationService;
        }
        protected override CoreResponse PageData(CoreRequest core_request)
        
        {
            try
            {
                var query= this.GetAll();
                if (!string.IsNullOrEmpty(core_request.DtRequest.Search.Value))
                {
                    var search = core_request.DtRequest.Search.Value;
                    query = query.Where(t => t.Name.Contains(search)||t.Remarks.Contains(search)||t.Industry.Contains(search)||t.Tel.Contains(search)||t.Type.Contains(search)||t.PublicTel.Contains(search));
                }
                
                //if (!string.IsNullOrEmpty(core_request.DtRequest.Search.Value))
                //{
                //    foreach (var item in core_request.DtRequest.Columns)
                //    {
                //        if (item.Data.Contains("Time") || item.Data.Contains("Date") || item.Data.Contains("Id"))
                //        {
                //            continue;
                //        }
                //        item.Search.Value = core_request.DtRequest.Search.Value;
                //    }
                //}
                //var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
                //var query = from item in dbcontext.CompanyInfo
                //            select new CompanyInfoDTO
                //            {
                //                Id = item.Id,
                //                Address = item.Address,
                //                BusinessScope = item.BusinessScope,
                //                BusinessStatus = item.BusinessStatus,
                //                BusinessTerm = item.BusinessTerm,
                //                City = item.City,
                //                CreateDate = item.CreateDate,
                //                CreditCode = item.CreditCode,
                //                District = item.District,
                //                EnglishName = item.EnglishName,
                //                IdentificationNumber = item.IdentificationNumber,
                //                Industry = item.Industry,
                //                LegalPerson = item.LegalPerson,
                //                Name = item.Name,
                //                NameUsedBefore = item.NameUsedBefore,
                //                NumberInsured = item.NumberInsured,
                //                OrganizationCode = item.OrganizationCode,
                //                PaidCapital = item.PaidCapital,
                //                PersonnelSize = item.PersonnelSize,
                //                Province = item.Province,
                //                PublicAddress = item.PublicAddress,
                //                PublicEmail = item.PublicEmail,
                //                PublicTel = item.PublicTel,
                //                PublicWebSite = item.PublicWebSite,
                //                RegisterAddress = item.RegisterAddress,
                //                RegisteredCapital = item.RegisteredCapital,
                //                RegistrationAuthority = item.RegistrationAuthority,
                //                RegistrationNo = item.RegistrationNo,
                //                TaxpayerQualification = item.TaxpayerQualification,
                //                Tel = item.Tel,
                //                Type = item.Type,
                //                Remarks = item.Remarks,
                //            };
                var result = base.PageDataWithQuery<CompanyInfoDTO>(core_request, query);
                List<CompanyInfoDTO> itemList = result.DtResponse.data as List<CompanyInfoDTO>;
                result.DtResponse.data = itemList;
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
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
                    CompanyInfoDTO companyInfoDTO = this.GetByID(Convert.ToInt32(Id));
                    if (companyInfoDTO == null) continue;
                    if (data.ContainsKey("Remarks"))
                    {
                        companyInfoDTO.Remarks = data["Remarks"].ToString();
                    }
                    companyInfoDTO.ModifyTime = DateTime.Now;
                    companyInfoDTO.Modifier = core_request.HttpContext.Session.GetString("User");
                    this.Update(companyInfoDTO);
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

        public CoreResponse CompanyAllPageData(CoreRequest core_request)
        {
            var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            //var query = from item in dbcontext.CompanyInfo
            //            join item1 in dbcontext.CompanyQualification
            //                   on item.Name equals item1.Name
            //            select new CompanyInfoDTO
            //            {
            //                Id = item.Id,
            //                Province = item.Province,
            //                City = item.City,
            //                District = item.District,
            //                Type = item.Type,
            //                Industry = item.Industry,
            //                Name = item.Name,
            //                LegalPerson = item.LegalPerson,
            //                CreateDate = item.CreateDate,
            //                Address = item.Address,
            //                Tel = item.Tel,
            //                PublicTel = item.PublicTel,
            //                PublicAddress = item.PublicAddress,
            //                PublicWebSite = item.PublicWebSite,
            //                PublicEmail = item.PublicEmail,
            //                BusinessScope = item.BusinessScope,
            //                RegisteredCapital = item.RegisteredCapital,
            //                PaidCapital = item.PaidCapital,
            //                BusinessStatus = item.BusinessStatus,
            //                CreditCode = item.CreditCode,
            //                RegistrationNo = item.RegistrationNo,
            //                IdentificationNumber = item.IdentificationNumber,
            //                OrganizationCode = item.OrganizationCode,
            //                RegistrationAuthority = item.RegistrationAuthority,
            //                BusinessTerm = item.BusinessTerm,
            //                TaxpayerQualification = item.TaxpayerQualification,
            //                PersonnelSize = item.PersonnelSize,
            //                NumberInsured = item.NumberInsured,
            //                NameUsedBefore = item.NameUsedBefore,
            //                EnglishName = item.EnglishName,
            //                RegisterAddress = item.RegisterAddress,

            //                City1 = item1.City,
            //                Code = item1.Code,
            //                ComprehensiveScore = item1.ComprehensiveScore,
            //                ContactAddress = item1.ContactAddress,
            //                EconomicType = item1.EconomicType,
            //                Email = item1.Email,
            //                EndDate = item1.EndDate,
            //                IssuingAuthority = item1.IssuingAuthority,
            //                OrganizationCode1 = item1.OrganizationCode,
            //                Province1 = item1.Province,
            //                QualificationType = item1.QualificationType,
            //                SafetyLicenseNo = item1.SafetyLicenseNo,
            //                ScopeLicense = item1.ScopeLicense,
            //                StartDate = item1.StartDate,
            //                Time = item1.Time,
            //                WebSite = item1.WebSite,
            //                ZipCode = item1.ZipCode,

            //                Remarks= item.Remarks,
            //                ModifyTime= item.ModifyTime,
            //                Modifier= item.Modifier,
            //            };
            var query = "SELECT A.*,B.Time as Time1,B.StartDate as StartDate1,B.EndDate as EndDate1,B.Code,B.EconomicType,B.Province AS Province1,B.City AS City1,B.Time,B.Email,B.WebSite,B.QualificationType,B.ContactAddress,B.ZipCode,B.SafetyLicenseNo,B.StartDate,B.EndDate,B.IssuingAuthority,B.ScopeLicense,B.OrganizationCode AS OrganizationCode1,B.ComprehensiveScore,null as access_token  FROM dbo.CompanyInfo A JOIN dbo.CompanyQualification B ON A.Name=B.Name";
            var result = base.PageDataWithSQL<CompanyInfoDTO>(core_request, query);
            //var result = base.PageDataWithQuery<CompanyInfoDTO>(core_request, query);
            List<CompanyInfoDTO> itemList = result.DtResponse.data as List<CompanyInfoDTO>;
            result.DtResponse.data = itemList;
            return result;
        }
    }
}
