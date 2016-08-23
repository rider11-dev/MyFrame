using MyFrame.Infrastructure.OptResult;
using MyFrame.RBAC.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core.ViewModel;
using WebApp.Controllers;
using WebApp.Core.Service.Interface;
using WebApp.Extensions.Filters;
using WebApp.Extensions.Session;
using MyFrame.Infrastructure.Common;
using WebApp.Core.Models;
using System.IO;
using MyFrame.Infrastructure.Images;

namespace WebApp.Areas.Public.Controllers
{
    public class UserDetailsController : BaseController
    {
        IUserDetailsService _usrDetailSrv;
        public UserDetailsController(IUserDetailsService usrDetailSrv, IOperationService optSrv)
            : base(optSrv)
        {
            _usrDetailSrv = usrDetailSrv;
        }

        //
        // GET: /Public/UserDetails/
        [LoginCheck]
        public ActionResult Index()
        {
            var usrId = Session.GetUserId();
            if (usrId == null)
            {
                throw new Exception("未找到登录用户信息");
            }
            var rst = _usrDetailSrv.GetDetailsById(usrId);
            if (rst.ResultType != OperationResultType.Success)
            {
                throw new Exception("未找到登录用户信息" + rst.Message);
            }

            return View(rst.AppendData as UserDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginCheck]
        public JsonResult Save(UserDetailsViewModel usrDetailsVM)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { code = OperationResultType.ParamError, message = base.ParseModelStateErrorMessage(ModelState) });
            }
            var usrDetails = OOMapper.Map<UserDetailsViewModel, UserDetails>(usrDetailsVM);
            var rst = _usrDetailSrv.Save(usrDetails);
            if (rst.ResultType != OperationResultType.Success)
            {
                return Json(new { code = rst.ResultType, message = rst.Message });
            }
            return Json(new { code = OperationResultType.Success, message = "保存成功" });
        }

        private string GetUploadVirtualDir(string dir)
        {
            return Request.ApplicationPath + "/upload/" + (string.IsNullOrEmpty(dir) ? "unknown" : dir);
        }

        [LoginCheck]
        public ActionResult EditAvatar()
        {
            //查找头像源图是否已存在
            var srcImgPath = "~/Content/images/blank.jpg";
            var usrDetail = Session.GetUserDetail();
            if (usrDetail != null)
            {
                var realPath = Server.MapPath(usrDetail.SrcImage);
                if (System.IO.File.Exists(realPath))
                {
                    srcImgPath = usrDetail.SrcImage;
                }
            }

            ViewData.Add("imgSrc", Url.Content(srcImgPath));
            return View();
        }

        [HttpPost]
        [LoginCheck]
        public JsonResult UploadFile()
        {
            JsonResult jsonRst = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            try
            {
                HttpPostedFileBase file = HttpContext.Request.Files["input_uploadFile"];
                string folder = HttpContext.Request["folder"];
                string virtualFolder = GetUploadVirtualDir(folder) + "/src";//虚拟路径
                string realFolder = Server.MapPath(virtualFolder);

                if (file == null)
                {
                    jsonRst.Data = new { code = OperationResultType.ParamError, message = "上传文件不能为空" };
                    return jsonRst;
                }

                if (!Directory.Exists(realFolder))
                {
                    Directory.CreateDirectory(realFolder);
                }

                //这里可以校验文件后缀（只允许指定类型文件上传）
                //保存
                string fileName = Session.GetUserId() + Path.GetExtension(file.FileName);
                file.SaveAs(realFolder + "/" + fileName);

                jsonRst.Data = new { code = OperationResultType.Success, message = "文件上传成功", data = new { folder = folder, filepath = virtualFolder + "/" + fileName } };
            }
            catch (Exception ex)
            {
                jsonRst.Data = new { code = OperationResultType.Error, message = ex.Message };
            }

            return jsonRst;
        }

        [HttpPost]
        [LoginCheck]
        public JsonResult CutAvatar(CutAvatarParams cutParams)
        {
            JsonResult jsonRst = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            if (cutParams == null)
            {
                jsonRst.Data = new { code = OperationResultType.ParamError, message = "图片裁剪参数不能为空" };
                return jsonRst;
            }
            if (string.IsNullOrEmpty(cutParams.imgSrcFilePath))
            {
                jsonRst.Data = new { code = OperationResultType.ParamError, message = "源图文件路径不能为空" };
                return jsonRst;
            }

            cutParams.imgSrcFileRealPath = Server.MapPath(cutParams.imgSrcFilePath);
            cutParams.imgAvatarFilePath = cutParams.imgSrcFilePath.Replace("src", "avatar");
            cutParams.imgAvatarRealPath = cutParams.imgSrcFileRealPath.Replace("src", "avatar");
            try
            {
                int usrId = (int)Session.GetUserId();
                var optRst = _usrDetailSrv.UpdateAvatar(cutParams, usrId);
                if (optRst.ResultType != OperationResultType.Success)
                {
                    jsonRst.Data = new { code = optRst.ResultType, message = optRst.Message };
                    return jsonRst;
                }
                //更新session
                var usrDetail = Session.GetUserDetail();
                if (usrDetail == null)
                {
                    optRst = _usrDetailSrv.Find(u => u.Id == usrId);
                    if (optRst.ResultType == OperationResultType.Success)
                    {
                        var lst = (List<UserDetails>)optRst.AppendData;
                        if (lst != null && lst.Count > 0)
                        {
                            usrDetail = lst[0];
                        }
                    }
                }
                if (usrDetail != null)
                {
                    usrDetail.SrcImage = cutParams.imgSrcFilePath;
                    usrDetail.AvatarImage = cutParams.imgAvatarFilePath;
                    Session.SetUserDetail(usrDetail);
                }

                jsonRst.Data = new { code = OperationResultType.Success, message = "头像修改成功" };
            }
            catch (Exception ex)
            {
                jsonRst.Data = new { code = OperationResultType.Error, message = ex.Message };
            }
            return jsonRst;
        }
    }
}