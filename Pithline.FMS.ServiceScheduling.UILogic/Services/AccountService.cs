using Pithline.FMS.BusinessLogic;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Services
{
    public class AccountService : IAccountService
    {
        public const string SignedInUserKey = "AccountService_UserInfo";
        public const string UserIdKey = "SS_UserId";
        public const string PasswordKey = "SS_Password";
        public const string PasswordVaultResourceName = "ServiceScheduling";

        ISessionStateService _sessionStateService;
        ICredentialStore _credentialStore;
        IIdentityService _identityService;

        UserInfo _signedInUser;
        string _userId;
        string _password;
        public AccountService(IIdentityService identityService, ISessionStateService sessionStateService, ICredentialStore credentialStore)
        {
            _identityService = identityService;
            _sessionStateService = sessionStateService;
            _credentialStore = credentialStore;
            if (_sessionStateService.SessionState.ContainsKey(SignedInUserKey))
            {
                _signedInUser = _sessionStateService.SessionState[SignedInUserKey] as UserInfo;
            }
            if (_sessionStateService.SessionState.ContainsKey(UserIdKey))
            {
                _userId = _sessionStateService.SessionState[UserIdKey].ToString();
            }
            if (_sessionStateService.SessionState.ContainsKey(PasswordKey))
            {
                _password = _sessionStateService.SessionState[PasswordKey].ToString();
            }
        }
        public BusinessLogic.UserInfo SignedInUser
        {
            get { return _signedInUser; }
        }

        async public Task<Tuple<UserInfo, string>> SignInAsync(string userId, string password, bool isCredentialStore)
        {
            try
            {
                var userInfo = new UserInfo { Name = userId, UserId = userId };
                var result = await _identityService.LogonAsync(userId, password);
                if (result.Item1 == null)
                {
                    return new Tuple<UserInfo, string>(null, result.Item2);
                }

                UserInfo previousUser = _signedInUser;
                _signedInUser = result.Item1.UserInfo;

                _sessionStateService.SessionState[SignedInUserKey] = _signedInUser;
                _userId = userId;
                _password = password;
                _sessionStateService.SessionState[UserIdKey] = userId;
                _sessionStateService.SessionState[PasswordKey] = password;

                if (isCredentialStore)
                {
                    _credentialStore.SaveCredentials(PasswordVaultResourceName, userId, password);
                }

                if (previousUser == null)
                {
                    RaiseUserChanged(_signedInUser, previousUser);
                }
                else if (_signedInUser != null && _signedInUser.UserId != previousUser.UserId)
                {
                    RaiseUserChanged(_signedInUser, previousUser);
                }
                return new Tuple<UserInfo, string>(_signedInUser, "");
            }
            catch (Exception ex)
            {
                return new Tuple<UserInfo, string>(null, ex.Message);
            }
        }

        private void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new BusinessLogic.EventArgs.UserChangedEventArgs(newUserInfo, oldUserInfo));
            }
        }

        public event EventHandler<BusinessLogic.EventArgs.UserChangedEventArgs> UserChanged;


        async public Task<Tuple<string,string>> VerifyUserCredentialsAsync()
        {
            var cred = _credentialStore.GetSavedCredentials(PasswordVaultResourceName);
            if (cred!=null)
            {
                cred.RetrievePassword();
                
                var result = await SignInAsync(cred.UserName, cred.Password, false);
                if (result.Item1 != null)
                {
                    
                return new Tuple<string, string>(cred.UserName, cred.Password);
                }
            }
            return null;

        }

        public void SignOut()
        {
            var previousUser = _signedInUser;
            _userId = null;
            _password = null;
            _sessionStateService.SessionState.Remove(SignedInUserKey);
            _sessionStateService.SessionState.Remove(UserIdKey);
            _sessionStateService.SessionState.Remove(PasswordKey);

            _credentialStore.RemoveSavedCredentials(PasswordVaultResourceName);
            RaiseUserChanged(_signedInUser, previousUser);
        }


    }
}
