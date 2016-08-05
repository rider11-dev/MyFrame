using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.RBAC.ViewModel
{
    public class OperationSimpleViewModel
    {
        public int Id { get; set; }
        public string OptCode { get; set; }
        public string OptName { get; set; }
        public int SortOrder { get; set; }
    }
}
