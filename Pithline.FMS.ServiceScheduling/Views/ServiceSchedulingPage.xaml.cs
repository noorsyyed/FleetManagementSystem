using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.Common;
using Pithline.FMS.ServiceScheduling.UILogic.AifServices;
using Pithline.FMS.ServiceScheduling.UILogic.ViewModels;
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

namespace Pithline.FMS.ServiceScheduling.Views
{
    public sealed partial class ServiceSchedulingPage : VisualStateAwarePage
    {

        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        ServiceSchedulingDetail serviceSchedulingDetail = null;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }
        public ServiceSchedulingPage()
        {
            this.InitializeComponent();
            serviceSchedulingDetail = ((ServiceSchedulingDetail)((ServiceSchedulingPageViewModel)this.DataContext).Model);
        }

    }
}
