using MyFrame.Core.Service;
using MyFrame.Infrastructure.Images;
using MyFrame.Infrastructure.OptResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Core.Models;

namespace WebApp.Core.Service.Interface
{
    public interface IUserDetailsService : IBaseService<UserDetails>
    {
        OperationResult GetDetailsById(int? id);
        OperationResult Save(UserDetails usrDetails);
        OperationResult UpdateAvatar(CutAvatarParams cutParams, int usrId);
    }
}
