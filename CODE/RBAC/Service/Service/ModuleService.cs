using MyFrame.Infrastructure.OptResult;
using MyFrame.Infrastructure.OrderBy;
using MyFrame.Infrastructure.Pagination;
using MyFrame.RBAC.Model;
using MyFrame.Model.Unit;
using MyFrame.RBAC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using MyFrame.RBAC.Repository;
using MyFrame.Service;

namespace MyFrame.RBAC.Service
{
    public class ModuleService : BaseService<Module>, IModuleService
    {
        private IModuleRepository _moduleRepository;
        private IUserRepository _usrRepository;
        const string Msg_SearchSimpleInfoByPage = "分页获取模块精简信息";
        public ModuleService(IUnitOfWork unitOfWork, IModuleRepository moduleRep, IUserRepository usrRep)
            : base(unitOfWork)
        {
            _moduleRepository = moduleRep;
            _usrRepository = usrRep;
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
                base.ProcessException(result, string.Format("根据模块编号获取{0}数据实体出错", base.EntityType), ex);
            }
            return result;
        }

        public OperationResult UpdateDetail(Module module)
        {
            OperationResult result = new OperationResult();
            if (module == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = "参数错误，模块实体不能为空";
                return result;
            }
            return base.Update(m => m.Id == module.Id, m => new Module
            {
                Name = module.Name,
                LinkUrl = module.LinkUrl,
                Icon = module.Icon,
                IsMenu = module.IsMenu,
                //ParentId = module.ParentId,
                //HasChild = _moduleRepository.Entities.Where(m1 => m1.ParentId == module.Id).Count() > 0,
                Enabled = module.Enabled,
                IsSystem = module.IsSystem,
                //SortOrder = module.SortOrder,
                Remark = module.Remark,
                LastModifier = module.LastModifier,
                LastModifyTime = module.LastModifyTime
            });
        }


        public OperationResult FindByPageWithFullInfo(Expression<Func<Module, bool>> where, Action<IOrderable<Module>> orderBy, PageArgs pageArgs)
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
                                IsMenu = module.IsMenu,
                                ParentId = module.ParentId,
                                ParentName = (p == null) ? "" : (p.Code + "|" + p.Name),
                                HasChild = module.HasChild,
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
                ProcessException(result, string.Format("分页获取{0}模块详细信息失败", EntityType), ex);
            }
            return result;
        }


        public OperationResult FindByPageWithSimpleInfo(Expression<Func<Module, bool>> where, Action<IOrderable<Module>> orderBy, PageArgs pageArgs)
        {
            OperationResult result = new OperationResult();
            try
            {
                //先分页
                var modulePaged = _moduleRepository.FindByPage(where, orderBy, pageArgs);
                //再连接
                var query = from module in modulePaged
                            select new
                            {
                                Id = module.Id,
                                Code = module.Code,
                                Name = module.Name,
                                LinkUrl = module.LinkUrl,
                                Enabled = module.Enabled,
                                Remark = module.Remark
                            };
                result.ResultType = OperationResultType.Success;
                result.AppendData = query.ToList();
            }
            catch (Exception ex)
            {
                base.ProcessException(result, string.Format(Msg_SearchSimpleInfoByPage + ",失败"), ex);
            }
            return result;
        }
    }
}
