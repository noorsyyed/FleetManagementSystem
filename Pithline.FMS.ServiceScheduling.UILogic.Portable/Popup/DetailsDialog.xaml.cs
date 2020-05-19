using Pithline.FMS.ServiceScheduling.UILogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.ServiceScheduling
{
    public sealed partial class DetailsDialog : ContentDialog
    {
        public DetailsDialog()
        {
            this.InitializeComponent();
            this.SizeChanged += DetailsDialog_SizeChanged;

            var b = Window.Current.Bounds;
            this.scrlViewTask.MaxHeight = b.Height - 100;
            this.scrlViewCust.MaxHeight = b.Height - 100;
        }

        void DetailsDialog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var b = Window.Current.Bounds;
            this.scrlViewTask.MaxHeight = b.Height - 100;
            this.scrlViewCust.MaxHeight = b.Height - 100;
        }

    }
}