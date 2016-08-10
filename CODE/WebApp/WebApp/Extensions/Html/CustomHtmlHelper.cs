using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Extensions.Html
{
    /// <summary>
    /// 自定义HtmlHelper扩展
    /// 注：需要在web.config注册当前命名空间
    /// </summary>
    public static class CustomHtmlHelper
    {
        /// <summary>
        /// 操作按钮
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        public static MvcHtmlString OptButton(this HtmlHelper helper, Operation opt)
        {
            if (opt == null)
            {
                return MvcHtmlString.Empty;
            }
            var btnBuilder = new TagBuilder("button");
            btnBuilder.MergeAttribute("type", "button");
            btnBuilder.GenerateId("btn_rbac_" + opt.ModuleId.ToString() + "_" + opt.OptCode);//btn_rbac_模块id_操作编号
            btnBuilder.MergeAttribute("id", opt.ModuleId.ToString() + "_" + opt.OptCode);
            btnBuilder.MergeAttribute("style", opt.CssStyle);
            btnBuilder.MergeAttribute("data-clickFunc", opt.ClickFunc);
            btnBuilder.MergeAttribute("data-submitUrl", opt.SubmitUrl);
            btnBuilder.MergeAttribute("data-moduleId", opt.ModuleId.ToString());
            btnBuilder.MergeAttribute("data-optId", opt.Id.ToString());
            btnBuilder.MergeAttribute("data-tag", string.IsNullOrEmpty(opt.Tag) ? opt.OptName : opt.Tag);
            btnBuilder.InnerHtml = string.Format("<span class='glyphicon {0}'>{1}</span>", opt.Icon, opt.OptName);
            btnBuilder.AddCssClass(opt.CssClass);

            btnBuilder.ToString(TagRenderMode.EndTag);//不能少


            return MvcHtmlString.Create(btnBuilder.ToString());
        }
    }
}