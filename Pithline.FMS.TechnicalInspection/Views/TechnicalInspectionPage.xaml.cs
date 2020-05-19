using Pithline.FMS.TechnicalInspection.Common;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
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

namespace Pithline.FMS.TechnicalInspection.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TechnicalInspectionPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public TechnicalInspectionPage()
        {
            this.InitializeComponent();

        }
        private void VisualStateAwarePage_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            TopAppBarShowValidationSummary.Visibility = Visibility.Collapsed;
        }

        private void TopAppBarShowValidationSummary_Closed(object sender, object e)
        {
            TopAppBarShowValidationSummary.Visibility = Visibility.Collapsed;
        }

    }
}
