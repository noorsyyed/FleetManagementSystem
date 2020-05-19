using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection.WindowsPhone.Views
{
    public sealed partial class InspectionDetailPage : VisualStateAwarePage
    {
        public InspectionDetailPage()
        {
            this.InitializeComponent();
          
            this.SizeChanged += InspectionDetailPage_SizeChanged;
        }

        void InspectionDetailPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((InspectionDetailPageViewModel)this.DataContext).BoundHeight = (Window.Current.Bounds.Height - 100) / 3;
            if (DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.Landscape ||

                DisplayInformation.GetForCurrentView().CurrentOrientation == DisplayOrientations.LandscapeFlipped)
            {
                ((InspectionDetailPageViewModel)this.DataContext).BoundWidth = Window.Current.Bounds.Width - 230;
            }
            else
            {
                ((InspectionDetailPageViewModel)this.DataContext).BoundWidth = Window.Current.Bounds.Width - 60;
            }
        }

    }
}
