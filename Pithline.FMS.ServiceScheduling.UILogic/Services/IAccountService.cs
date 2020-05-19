using Pithline.FMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Services
{
  public  interface IAccountService
    {
         UserInfo SignedInUser { get; }
         Task<Tuple<UserInfo, string>> SignInAsync(string userId, string password, bool isCredentialStore);
         void SignOut();

         event EventHandler<BusinessLogic.EventArgs.UserChangedEventArgs> UserChanged;

         Task<Tuple<string,string>> VerifyUserCredentialsAsync();

    }
}
