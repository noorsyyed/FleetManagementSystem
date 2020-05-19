using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services
{
    public interface IUserService
    {
        Task<UserInfo> GetUserInfoAsync(string userId);

        Task<AccessToken> ValidateUserAsync(string userName, string password);
        
    }
}
