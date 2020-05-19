using Pithline.FMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.TechnicalInspection.UILogic.Services
{
    public interface IAccountService
    {
        Task<Tuple<UserInfo, string>> SignInAsync(string userId, string password, bool isCredentialStore);

        UserInfo SignedInUser { get; }

        Tuple<string,string> VerifyUserCredentialsAsync();

        void SignOut();

        event EventHandler<Pithline.FMS.BusinessLogic.EventArgs.UserChangedEventArgs> UserChanged;

    }
}
