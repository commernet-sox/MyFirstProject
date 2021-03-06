﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Models;

namespace WFWebProject.Profile
{
    public class DTOProfile : AutoMapper.Profile
    {
        public DTOProfile() : base()
        {

            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<MenuInfoDTO,MenuInfo>();
            CreateMap<MenuInfo,MenuInfoDTO>();


            CreateMap<UserMenuRoleDTO, UserMenuRole>();
            CreateMap<UserMenuRole, UserMenuRoleDTO>();

            CreateMap<CodeMasterDTO,CodeMaster>();
            CreateMap<CodeMaster,CodeMasterDTO>();

            CreateMap<ConstructorInfoDTO,ConstructorInfo>();
            CreateMap<ConstructorInfo, ConstructorInfoDTO>();

            CreateMap<CompanyInfoDTO,CompanyInfo>();
            CreateMap<CompanyInfo, CompanyInfoDTO>();

            CreateMap<CompanyQualificationDTO,CompanyQualification>();
            CreateMap<CompanyQualification, CompanyQualificationDTO>();
        }
    }
}
