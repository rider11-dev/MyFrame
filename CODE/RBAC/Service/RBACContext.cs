using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Service
{
    public class RBACContext
    {
        public static User CurrentUser { get; set; }
    }
}
