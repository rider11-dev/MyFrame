﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewModels.RBAC
{
    public class ModuleTreeNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public int sort { get; set; }
        public List<ModuleTreeNode> nodes { get; set; }
    }
}