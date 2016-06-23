using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MyFrame.Model
{
    public interface IKey<TKey>
    {
        [Key]
        TKey Id { get; set; }
    }
}
