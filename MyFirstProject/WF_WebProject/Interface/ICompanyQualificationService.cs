﻿using Core.WebServices.Interface;
using Core.WebServices.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Models;

namespace WFWebProject.Interface
{
    public interface ICompanyQualificationService : IBase<CompanyQualification, CompanyQualificationDTO, int>, IDatatable
    {
        CoreResponse EditData(string Id, CoreRequest core_request);
    }
}
