using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFrame.RBAC.ViewModel
{
    public class UserDetailViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool Enabled { get; set; }

        public int? Creator { get; set; }

        public string CreatorName { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? LastModifier { get; set; }
        public string LastModifierName { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public string Roles { get; set; }
    }
}
