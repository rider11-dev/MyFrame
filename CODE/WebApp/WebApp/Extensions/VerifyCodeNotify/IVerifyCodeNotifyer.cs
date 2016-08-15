using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Extensions.VerifyCodeNotify
{
    public interface IVerifyCodeNotifyer<TNotifyMode> where TNotifyMode : class
    {
        /// <summary>
        /// 以指定方式通知验证码
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="args">通知方式相关参数</param>
        void Notify(string verifyCode, TNotifyMode args);
    }
}