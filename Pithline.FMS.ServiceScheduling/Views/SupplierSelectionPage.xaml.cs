using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.ServiceScheduling.Common;
using Newtonsoft.Json;
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
using System.Reflection;
using Microsoft.Practices.Prism.StoreApps;
using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.UILogic.ViewModels;
using Pithline.FMS.ServiceScheduling.UILogic.AifServices;
using Windows.UI.Xaml.Documents;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Pithline.FMS.ServiceScheduling.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SupplierSelectionPage : Microsoft.Practices.Prism.StoreApps.VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public SupplierSelectionPage()
        {
            this.InitializeComponent();
        }
    }
}