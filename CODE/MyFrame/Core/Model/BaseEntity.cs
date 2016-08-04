using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Core.Model
{
    public class BaseEntity<TKey> : IKey<TKey>
    {
        [Description("主键")]
        [Key]
        public TKey Id { get; set; }
    }
}
