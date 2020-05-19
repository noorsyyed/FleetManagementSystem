using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Pithline.FMS.DocumentDelivery.Views
{
    public sealed partial class AddCustomerPage : SettingsFlyout
    {
        public AddCustomerPage()
        {
            this.InitializeComponent();
        }

        private void CellNumber_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if ((e.Key < VirtualKey.Number0) || (e.Key > VirtualKey.Number9))
            {
                e.Handled = true;
            }
        }
    }
}
