using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection.Views
{
    public sealed partial class UserProfile : ContentDialog
    {
        public INavigationService _navigationService;
        public UserProfile(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.InitializeComponent();
       
            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
            {
                this.DataContext = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.RoamingSettings.Values.Remove(Constants.ACCESSTOKEN);
            ApplicationData.Current.RoamingSettings.Values.Remove(Constants.USERINFO);
            this._navigationService.Navigate("Login", string.Empty);
            _navigationService.ClearHistory();
            this.Hide();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }

    }
}
