using MyFrame.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.Model
{
    /// <summary>
    /// 模块---实体
    /// </summary>
    [Description("用户——角色关系")]
    [Table("UserRoleRelation")]
    public class UserRoleRelation : IKey<int>
    {
        public UserRoleRelation() { }

        [Description("主键")]
        public int Id { get; set; }

        [Description("用户id")]
        public int UserId { get; set; }

        [Description("角色id")]
        public int RoleId { get; set; }
    }
}
