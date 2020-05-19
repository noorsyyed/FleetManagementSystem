using Pithline.FMS.BusinessLogic;
using Pithline.FMS.DocumentDelivery.Common;
using Pithline.FMS.DocumentDelivery.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.DocumentDelivery.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CollectionOrDeliveryDetailsPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public CollectionOrDeliveryDetailsPage()
        {
            this.InitializeComponent();

        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sfDataGrid.SelectAll();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            sfDataGrid.ClearSelections(false);
        }

    }
}
