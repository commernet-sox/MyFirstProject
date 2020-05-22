using AutoMapper;
using Core.Database.Extension;
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
    public class UserMenuRoleService : BaseService<UserMenuRole, DataContext, UserMenuRoleDTO, int>, IUserMenuRoleService
    {
        private IMenuInfoService _menuInfoService;
        private IUserService _userService;
        public UserMenuRoleService(IRepository<UserMenuRole, DataContext> Repository, IMapper mapper,IMenuInfoService menuInfoService,IUserService userService) : base(Repository, mapper)
        {
            _menuInfoService = menuInfoService;
            _userService = userService;
        }
        protected override CoreResponse PageData(CoreRequest core_request)
        {
            
            var dbcontext = base.Repository.SlaveUnitOfWork.DbContext;
            var query = from A in dbcontext.UserMenuRole  
                        join B in dbcontext.User 
                        on Convert.ToInt32(A.UserId) equals B.Id 
                        select  new UserMenuRoleDTO
                {
                    UserId = A.UserId,
                    UserName = B.UserName,
                    Content = string.Join(",", A.Content.Distinct())
                };

            var result = base.PageDataWithQuery(core_request, query);
            List<UserMenuRoleDTO> list = result.DtResponse.data as List<UserMenuRoleDTO>;
            if (list != null && list.Count > 0)
            {
                //List<string> p_list = new List<string>();
                List<string> warehouseUids = new List<string>();

                foreach (var item in list)
                {
                    warehouseUids.AddRange(item.Content.Split(',').ToList());
                }
                warehouseUids = warehouseUids.Distinct().ToList();
                var userPermissions = _menuInfoService.GetAll().Where(w => warehouseUids.Contains(w.Id.ToString())).ToList();
                foreach (var r in list)
                {
                    List<MenuInfoDTO> permissions = new List<MenuInfoDTO>();
                    var warehouseIds = userPermissions.Where(t => r.Content.Split(',').Contains(t.Id.ToString()))
                        .Select(t => t.Id).Distinct();

                    //父节点
                    foreach (var warehouseId in warehouseIds)
                    {
                        var wares = userPermissions.Where(t => t.Id == warehouseId);
                        var ware = wares.FirstOrDefault().Clone() as MenuInfoDTO;
                        ware.ContentId = "M#" + ware.TitleId;
                        permissions.Add(ware);
                    }

                    var uids = r.Content.Split(',');
                    //子节点
                    foreach (var wa in uids)
                    {
                        var wares = userPermissions.Where(t => t.Id == Convert.ToInt32(wa));
                        if (wares.Count() > 0)
                        {
                            var ware = wares.FirstOrDefault().Clone() as MenuInfoDTO;
                            ware.ContentId = "D#" + ware.Id;
                            permissions.Add(ware);
                        }

                    }
                    r.Contents = permissions;
                }

                var WarehouseItems = this._menuInfoService.GetAll().Where(w => warehouseUids.Contains(w.Id.ToString())).GroupBy(t => t.TitleId).ToList();
                List<Tuple<string, string>> tuples = new List<Tuple<string, string>>();
                foreach (var p in WarehouseItems)
                {
                    var mx = p.FirstOrDefault();
                    tuples.Add(new Tuple<string, string>("M#" + mx.ContentId, mx.Content));
                    foreach (var q in p)
                    {
                        tuples.Add(new Tuple<string, string>("D#" + q.Id.ToString(), q.ContentId + "-" + q.Content));
                    }
                }
                var users = this._userService.GetAll()
                    .Select(t => new { value = t.Id, label = t.UserName }).Distinct().Future();
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("UserId", users);
                options.Add("Contents[].ContentId", tuples.Select(s => new { value = s.Item1, label = s.Item2 }));
                result.DtResponse.data = list;
                result.DtResponse.options = options;
            }
            return result;
        }
        protected override CoreResponse Create(CoreRequest core_request)
        {
            CoreResponse core_response = new CoreResponse(core_request);
            
            foreach (var item in core_request.DtRequest.Data)
            {
                string key = item.Key;
                List<Dictionary<string, object>> list_pair = new List<Dictionary<string, object>>();
                var pair = item.Value as Dictionary<string, object>;
                UserMenuRoleDTO orgin;

                orgin = new UserMenuRoleDTO();
                base.ConvertDictionaryToObject(orgin, pair, core_response.DtResponse.fieldErrors);
                if (core_response.DtResponse.fieldErrors != null && core_response.DtResponse.fieldErrors.Count > 0)
                    return core_response;

                if (this.GetAll().Where(t => t.UserId == orgin.UserId).Count() > 0)
                {
                    core_response.DtResponse.error += "该账号已存在权限数据,无法新增请编辑!";
                }
                else
                {

                    if (orgin.Contents == null)
                    {
                        core_response.DtResponse.error += "未勾选任何权限无法新增!";
                    }
                    else
                    {
                        //保存对应用户名
                        //orgin.UserName = _userService.GetAll().Where(t => t.LoginName == orgin.UserId).Select(t => t.AliasName).FirstOrDefault();

                        DBResult dbresult;
                        //orgin.CreateTime = DateTime.Now;
                        orgin.Creator = "123";
                        List<UserMenuRoleDTO> userDataRoleDTOs = new List<UserMenuRoleDTO>();
                        var validData = orgin.Contents.Where(t => t.ContentId.ToString().Contains("D#"));
                        foreach (var warehouse in validData)
                        {
                            var data = orgin.Clone() as UserMenuRoleDTO;
                            data.Content = warehouse.ContentId.ToString().Replace("D#", "");
                            userDataRoleDTOs.Add(data);
                        }
                        if (userDataRoleDTOs.Count == 0)
                        {
                            core_response.DtResponse.error += "请选择具体货主部门权限!";
                        }
                        else
                        {
                            dbresult = this.AddRange(userDataRoleDTOs.ToArray());
                            if (dbresult.Code != 0)
                            {
                                core_response.DtResponse.error += dbresult.ErrMsg;
                            }
                        }
                    }
                }
            }
            return core_response;
        }

        protected override CoreResponse Edit(CoreRequest core_request)
        {
            CoreResponse core_response = new CoreResponse(core_request);
            //UserDTO loginsession = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>(core_request.HttpContext.Session.GetString("User"));
            foreach (var item in core_request.DtRequest.Data)
            {
                string key = item.Key;
                List<Dictionary<string, object>> list_pair = new List<Dictionary<string, object>>();
                var pair = item.Value as Dictionary<string, object>;
                UserMenuRoleDTO orgin;

                orgin = new UserMenuRoleDTO();
                base.ConvertDictionaryToObject(orgin, pair, core_response.DtResponse.fieldErrors);
                if (core_response.DtResponse.fieldErrors != null && core_response.DtResponse.fieldErrors.Count > 0)
                    return core_response;

                if (orgin.Contents == null)
                {
                    var deleteIds = this.GetAll().Where(t => t.UserId == orgin.UserId).Select(t => t.Id).ToArray();
                    DBResult dbresult = this.BatchDelete(d => deleteIds.Contains(d.Id));
                    if (dbresult.Code != 0)
                    {
                        core_response.DtResponse.error += dbresult.ErrMsg;
                    }
                }
                else
                {
                    DBResult dbresult;
                    //orgin.CreateTime = DateTime.Now;
                    orgin.Creator = "123";
                    List<UserMenuRoleDTO> userDataRoleDTOs = new List<UserMenuRoleDTO>();
                    var validData = orgin.Contents.Where(t => t.ContentId.Contains("D#"));
                    foreach (var warehouse in validData)
                    {
                        var data = orgin.Clone() as UserMenuRoleDTO;
                        data.Content = warehouse.ContentId.Replace("D#", "");
                        userDataRoleDTOs.Add(data);
                    }
                    if (userDataRoleDTOs.Count == 0)
                    {
                        core_response.DtResponse.error += "请选择具体货主部门权限!";
                    }
                    else
                    {
                        var deleteIds = this.GetAll().Where(t => t.UserId == orgin.UserId).Select(t => t.Id).ToArray();
                        dbresult = this.BatchDelete(d => deleteIds.Contains(d.Id));
                        if (dbresult.Code != 0)
                        {
                            core_response.DtResponse.error += dbresult.ErrMsg;
                        }
                        else
                        {
                            dbresult = this.AddRange(userDataRoleDTOs.ToArray());
                            if (dbresult.Code != 0)
                            {
                                core_response.DtResponse.error += dbresult.ErrMsg;
                            }
                        }
                    }
                }
            }
            return core_response;
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
