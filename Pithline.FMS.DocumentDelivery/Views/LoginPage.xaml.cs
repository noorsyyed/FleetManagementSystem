
using Pithline.FMS.DocumentDelivery.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Input;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : VisualStateAwarePage
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }
        private void PasswordBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            var vm = (LoginPageViewModel)this.DataContext;
            vm.ErrorMessage = string.Empty;
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.btnLogin.Command.Execute(null);
            }
        }
    }
}
