using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.Extension;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Model;
using MyFrame.Core.Model;
using MyFrame.RBAC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.RBAC.Repository;
using MyFrame.Core.Service;
using MyFrame.RBAC.Repository.Interface;
using MyFrame.Core.UnitOfWork;
using MyFrame.RBAC.Service.Interface;

namespace MyFrame.RBAC.Service.Impl
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        private IModuleRepository _moduleRepository;
        private IUserRepository _usrRepository;
        private IRolePermissionRepository _rolePermissionRep;
        private IRoleRepository _roleRep;
        IOperationRepository _optRep;
        const string Msg_BeforeAdd = "保存前校验";
        const string Msg_AfterAdd = "保存后操作";
        const string Msg_BeforeDelete = "删除前校验";
        const string Msg_AfterDelete = "删除后操作";
        const string Msg_SearchSimpleInfoByPage = "分页获取模块精简信息";
        const string Msg_SearchSimpleInfoByRoles = "根据角色获取模块精简信息";
        const string Msg_UpdateDetail = "更新模块详细信息";
        public ModuleService(IUnitOfWork unitOfWork, IModuleRepository moduleRep,
            IUserRepository usrRep,
            IRolePermissionRepository rolePermissionRep,
            IRoleRepository roleRep,
            IOperationRepository optRep)
            : base(unitOfWork)
        {
            _moduleRepository = moduleRep;
            _usrRepository = usrRep;
            _rolePermissionRep = rolePermissionRep;
            _roleRep = roleRep;
            _optRep = optRep;
        }

        public OperationResult FindByModuleCode(string moduleCode)
        {
            OperationResult result = new OperationResult();
            if (string.IsNullOrEmpty(moduleCode))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，模块编号不能为空";
                return result;
            }
            try
            {
                var data = _moduleRepository.Find(m => m.Code == moduleCode);
                result.ResultType = OperationResultType.Success;
                result.Message = "模块编号查询成功";
                result.AppendData = data.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, string.Format("根据模块编号获取{0}数据实体出错", base.EntityType), ex);
            }
            return result;
        }

        public OperationResult UpdateDetail(Module module)
        {
            OperationResult result = new OperationResult();
            if (module == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + ",参数错误，模块实体不能为空";
                return result;
            }

            if (module.Id == module.ParentId)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + ",参数错误，上级模块不能选择自身";
                return result;
            }

            result = base.Exists(m => m.Id == module.Id);
            if (result.ResultType != OperationResultType.Success)
            {
                return result;
            }
            if (Convert.ToBoolean(result.AppendData) == false)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + "失败，指定模块不存在";
                return result;
            }
            result = base.Update(m => m.Id == module.Id, m => new Module
            {
                Name = module.Name,
                LinkUrl = module.LinkUrl,
                Icon = module.Icon,
                ParentId = module.ParentId,
                Enabled = module.Enabled,
                SortOrder = module.SortOrder,
                Remark = module.Remark,
                LastModifier = module.LastModifier,
                LastModifyTime = module.LastModifyTime
            });
            return result;
        }


        public OperationResult FindByPageWithFullInfo(Expression<Func<Module, bool>> where, Func<IQueryable<Module>, IOrderedQueryable<Module>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                var modulePaged = _moduleRepository.FindByPage(where, orderBy, pageArgs);
                var query = from module in modulePaged
                            join parent in _moduleRepository.Entities on module.ParentId equals parent.Id into parentQuery
                            from p in parentQuery.DefaultIfEmpty()
                            join creator in _usrRepository.Entities on module.Creator equals creator.Id into creatorQuery
                            from c in creatorQuery.DefaultIfEmpty()
                            join lastmodifier in _usrRepository.Entities on module.LastModifier equals lastmodifier.Id into lastmodifierQuery
                            from m in lastmodifierQuery.DefaultIfEmpty()
                            select new
                            {
                                Id = module.Id,
                                Code = module.Code,
                                Name = module.Name,
                                LinkUrl = module.LinkUrl,
                                Icon = module.Icon,
                                SortOrder = module.SortOrder,
                                ParentId = module.ParentId,
                                ParentName = (p == null) ? "" : (p.Code + "|" + p.Name),
                                Enabled = module.Enabled,
                                IsSystem = module.IsSystem,
                                Remark = module.Remark,
                                Creator = module.Creator,
                                CreatorName = c.UserName,
                                CreateTime = module.CreateTime,
                                LastModifier = module.LastModifier,
                                LastModifierName = m.UserName,
                                LastModifyTime = module.LastModifyTime
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, string.Format("分页获取{0}模块详细信息失败", EntityType), ex);
            }
            return result;
        }


        public OperationResult FindByPageWithSimpleInfo(Expression<Func<Module, bool>> where, Func<IQueryable<Module>, IOrderedQueryable<Module>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                //先分页
                var modulePaged = _moduleRepository.FindByPage(where, orderBy, pageArgs);
                //再连接
                var query = from module in modulePaged
                            select new ModuleSimpleViewModel
                            {
                                Id = module.Id,
                                Code = module.Code,
                                Name = module.Name,
                                LinkUrl = module.LinkUrl,
                                Icon = module.Icon,
                                SortOrder = module.SortOrder,
                                ParentId = module.ParentId,
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, string.Format(Msg_SearchSimpleInfoByPage + ",失败"), ex);
            }
            return result;
        }

        public OperationResult FindByRolesWithSimpleInfo(int[] roleIds)
        {
            OperationResult result = new OperationResult();
            if (roleIds == null || roleIds.Length < 1)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，角色不能为空";
                return result;
            }
            var query = from module in _moduleRepository.Entities
                        join per in _rolePermissionRep.Entities on module.Id equals per.PermissionId
                        join role in _roleRep.Entities on per.RoleId equals role.Id
                        where roleIds.Contains(per.RoleId)
                            && module.Enabled == true
                            && role.Enabled == true
                        select new ModuleSimpleViewModel
                        {
                            Id = module.Id,
                            Code = module.Code,
                            Name = module.Name,
                            LinkUrl = module.LinkUrl,
                            Icon = module.Icon,
                            SortOrder = module.SortOrder,
                            ParentId = module.ParentId,
                        };
            try
            {
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.Distinct().ToList();//去重
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, string.Format(Msg_SearchSimpleInfoByRoles + ",失败"), ex);
            }
            return result;
        }

        protected override OperationResult OnBeforeAdd(Module entity)
        {
            OperationResult result = new OperationResult();
            if (entity == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "实体不能为空");
                return result;
            }
            //1、校验模块编号是否已存在
            bool check = _moduleRepository.Exists(m => m.Code == entity.Code);
            if (check)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = string.Format("{0}失败，{1}", Msg_BeforeAdd, "模块编号已存在");
                return result;
            }
            result.ResultType = OperationResultType.Success;
            return result;
        }
        protected override OperationResult OnBeforeDelete(Expression<Func<Module, bool>> where)
        {
            OperationResult result = new OperationResult { ResultType = OperationResultType.Success };
            var modulesToDel = _moduleRepository.Find(where);
            //1、模块是否包含子模块
            var query = from module in _moduleRepository.Entities
                        join mToDel in modulesToDel on module.ParentId equals mToDel.Id
                        select 1;
            if (query.Count() > 0)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = Msg_BeforeDelete + "失败，要删除的模块包含下级";
                return result;
            }
            //2、模块是否包含操作
            query = from module in modulesToDel
                    join opt in _optRep.Entities on module.Id equals opt.ModuleId
                    select 1;
            if (query.Count() > 0)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = Msg_BeforeDelete + "失败，要删除的模块包含操作";
                return result;
            }

            //3、检查模块是否被引用
            var perType = PermissionType.Module.ToInt();
            query = from per in _rolePermissionRep.Entities
                    join module in modulesToDel on per.PermissionId equals module.Id
                    where per.PerType == perType
                    select 1;
            if (query.Count() > 0)
            {
                result.ResultType = OperationResultType.CheckFailedBeforeProcess;
                result.Message = Msg_BeforeDelete + "失败，模块已被分配到角色";
            }
            return result;
        }

    }
}
