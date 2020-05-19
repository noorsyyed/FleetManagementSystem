using Pithline.FMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DocumentDelivery.UILogic.Services
{
   public interface IIdentityService
    {
       Task<Tuple<CDLogonResult, string>> LogonAsync(string userId, string password);

       Task<bool> VerifyAcitveSessionAsync(string userId);

    }
}
