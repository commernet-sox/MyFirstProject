﻿using Core.WebServices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.DTO;
using WFWebProject.Models;

namespace WFWebProject.Interface
{
    public interface IMenuInfoService : IBase<MenuInfo, MenuInfoDTO, int>, IDatatable
    {
    }
}
