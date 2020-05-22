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
    public class UserService : BaseService<User, DataContext, UserDTO, int>, IUserService
    {
        public UserService(IRepository<User, DataContext> Repository, IMapper mapper) : base(Repository, mapper)
        {

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
