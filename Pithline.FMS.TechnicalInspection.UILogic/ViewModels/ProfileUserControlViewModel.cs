using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.TechnicalInspection.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace Pithline.FMS.TechnicalInspection.UILogic.ViewModels
{
    public class ProfileUserControlViewModel : ViewModel
    {
        INavigationService _navigationService;
        IAccountService _accountService;
        public ProfileUserControlViewModel(INavigationService navigationService, IAccountService accountService)
        {
            _navigationService = navigationService;
            _accountService = accountService;

            UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
            GetNetworkStatus();
            LogoutCommand = new DelegateCommand(() =>
            {
                _accountService.SignOut();
                _navigationService.Navigate("Login", string.Empty);
                _navigationService.ClearHistory();
            });
        }

        

        private string networkIcon;

        public string NetworkIcon
        {
            get { return networkIcon; }
            set { SetProperty(ref networkIcon, value); }
        }

        private UserInfo userInfo;

        public UserInfo UserInfo
        {
            get { return userInfo; }
            set { SetProperty(ref userInfo, value); }
        }


        public void GetNetworkStatus()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkInformation.NetworkStatusChanged += (s) =>
                {
                    if (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => { NetworkIcon = "ms-appx:///Assets/NetConnected.png"; });
                    else
                        Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => { NetworkIcon = "ms-appx:///Assets/NetDisconnected.png"; });

                };
                if (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                    NetworkIcon = "ms-appx:///Assets/NetConnected.png";
                else
                    NetworkIcon = "ms-appx:///Assets/NetDisconnected.png";
            }
            catch (Exception ex)
            {

                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        public ICommand LogoutCommand { get; private set; }

    }
}
