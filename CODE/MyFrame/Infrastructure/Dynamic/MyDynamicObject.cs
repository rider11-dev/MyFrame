using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace MyFrame.Infrastructure.Dynamic
{
    /// <summary>
    /// 自定义动态类，可动态添加属性
    /// </summary>
    public class MyDynamicObject : DynamicObject
    {
        Dictionary<string, object> map;
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (map != null)
            {
                string name = binder.Name;
                object value;
                if (map.TryGetValue(name, out value))
                {
                    result = value;
                    return true;
                }
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder.Name == "set" && binder.CallInfo.ArgumentCount == 2)
            {
                string name = args[0] as string;
                if (name == null)
                {
                    result = null;
                    return false;
                }
                if (map == null)
                {
                    map = new Dictionary<string, object>();
                }
                object value = args[1];
                map.Add(name, value);
                result = value;
                return true;
            }

            return base.TryInvokeMember(binder, args, out result);
        }
    }
}
