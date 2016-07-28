using MyFrame.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.Model
{
    [Description("角色——权限关系")]
    [Table("RolePermission")]
    public class RolePermission : IKey<int>
    {
        public RolePermission() { }

        [Description("主键")]
        public int Id { get; set; }

        [Description("角色id")]
        public int RoleId { get; set; }

        [Description("权限id")]
        public int PermissionId { get; set; }

        [Description("权限类型")]
        public int PerType { get; set; }
    }
}
