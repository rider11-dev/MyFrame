using MyFrame.Core.Service;
using MyFrame.Core.UnitOfWork;
using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Model;
using MyFrame.RBAC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Models;
using WebApp.Core.Repository.Interface;
using WebApp.Core.Service.Interface;
using WebApp.Core.ViewModel;

namespace WebApp.Core.Service.Impl
{
    public class UserDetailsService : BaseService<UserDetails>, IUserDetailsService
    {
        const string Msg_FindById = "根据id获取用户详细信息";
        const string Msg_UpdateDetail = "更新用户详细信息";

        IUserDetailsRepository _usrDetailRep;
        IUserRepository _usrRep;
        public UserDetailsService(IUnitOfWork unitOfWork, IUserDetailsRepository usrDetailRep, IUserRepository usrRep)
            : base(unitOfWork)
        {
            _usrDetailRep = usrDetailRep;
            _usrRep = usrRep;
        }

        public OperationResult GetDetailsById(int? id)
        {
            OperationResult rst = new OperationResult();
            if (id == null)
            {
                rst.ResultType = OperationResultType.ParamError;
                rst.Message = Msg_FindById + "失败，用户id不能为空";
                return rst;
            }
            //1、获取用户基本信息
            var usr = _usrRep.FindBySelector(u => u.Id == (int)id, u => new { Id = u.Id, UserName = u.UserName, Email = u.Email }).FirstOrDefault();
            if (usr == null)
            {
                rst.ResultType = OperationResultType.QueryNull;
                rst.Message = Msg_FindById + "失败，未找到指定用户";
                return rst;
            }
            var usrDetailVM = new UserDetailsViewModel { Id = usr.Id, UserName = usr.UserName, Email = usr.Email };
            //2、获取用户详细信息
            var usrDetail = _usrDetailRep.Find(u => u.Id == (int)id).FirstOrDefault();
            if (usrDetail != null)
            {
                usrDetailVM.NickName = usrDetail.NickName;
                usrDetailVM.BirthDate = usrDetail.BirthDate;
                usrDetailVM.Age = usrDetail.Age;
                usrDetailVM.Telephone = usrDetail.Telephone;
                usrDetailVM.Address = usrDetail.Address;
                usrDetailVM.Interests = usrDetail.Interests;
                usrDetailVM.HeadImage = usrDetail.HeadImage;
                usrDetailVM.PersonalNote = usrDetail.PersonalNote;

            }
            rst.ResultType = OperationResultType.Success;
            rst.AppendData = usrDetailVM;
            return rst;
        }

        public OperationResult Save(UserDetails usrDetails)
        {
            OperationResult result = new OperationResult();
            if (usrDetails == null)
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + ",参数错误，用户详细信息实体不能为空";
                return result;
            }
            if (!_usrRep.Exists(u => u.Id == usrDetails.Id))
            {
                result.ResultType = OperationResultType.ParamError;
                result.Message = Msg_UpdateDetail + ",指定用户不存在";
                return result;
            }
            bool detailExists = _usrDetailRep.Exists(u => u.Id == usrDetails.Id);
            try
            {
                result = !detailExists ?
                    base.Add(usrDetails) :
                    base.Update(u => u.Id == usrDetails.Id, u => new UserDetails
                {
                    NickName = usrDetails.NickName,
                    HeadImage = usrDetails.HeadImage,
                    BirthDate = usrDetails.BirthDate,
                    Age = usrDetails.Age,
                    Telephone = usrDetails.Telephone,
                    Address = usrDetails.Address,
                    Interests = usrDetails.Interests,
                    PersonalNote = usrDetails.PersonalNote
                });
            }
            catch (Exception ex)
            {
                base.ProcessException(ref result, Msg_UpdateDetail, ex);
            }
            return result;
        }
    }
}
