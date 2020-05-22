using Autofac;
using Core.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WFWebProject.Interface;
using WFWebProject.Models;
using WFWebProject.Profile;
using WFWebProject.Service;

namespace WFWebProject
{
    public class ServiceModules: Module
    {
        static ServiceModules()
        {
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DTOProfile>().As<AutoMapper.Profile>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<DatabaseUniofwork>().As<IUnitOfWork<DataContext>>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>)).InstancePerLifetimeScope();
            builder.RegisterType<CodeMasterService>().As<ICodeMasterService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyInfoService>().As<ICompanyInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<ConstructorInfoService>().As<IConstructorInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyQualificationService>().As<ICompanyQualificationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuInfoService>().As<IMenuInfoService>().InstancePerLifetimeScope();
            builder.RegisterType<UserMenuRoleService>().As<IUserMenuRoleService>().InstancePerLifetimeScope();
        }

    }
}
