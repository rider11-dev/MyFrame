using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFrame.Core.Configure
{
    public interface IMapper
    {
        void Map(IMapperConfiguration cfg);
    }
}
