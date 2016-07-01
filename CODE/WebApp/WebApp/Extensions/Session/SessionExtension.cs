using MyFrame.Model.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApp.Extensions.Session
{
    public static class SessionExtension
    {
        public const string KEY_USER_ID = "UserId";
        public const string KEY_USER_NAME = "UserName";

        public static int? GetUserId(this HttpSessionStateBase state)
        {
            return state.Get<int?>(KEY_USER_ID);
        }

        public static void SetUser(this HttpSessionStateBase state, User usr)
        {
            state.Set(KEY_USER_ID, usr.Id);
            state.Set(KEY_USER_NAME, usr.UserName);
        }

        public static void Set(this HttpSessionStateBase state, string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            state.Add(key, value);
        }

        public static TResult Get<TResult>(this HttpSessionStateBase state, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(TResult);
            }
            return (TResult)state[key];
        }
    }
}