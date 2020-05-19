using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ComponentsDetailPage : VisualStateAwarePage
    {
        ComponentsDetailPageViewModel vm;
        private SnapshotsViewer _snapShotsPopup;
        public ComponentsDetailPage()
        {
            this.InitializeComponent();
           
            this.vm = (ComponentsDetailPageViewModel)this.DataContext;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var b = Window.Current.Bounds;
            ImagesPivot.Width = b.Width;
        }

        private void TasksPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.vm.SelectedMaintenanceRepair != null)
            {
                if (((Pivot)sender).SelectedIndex == 1)
                {
                    this.vm.SelectedMaintenanceRepair.IsMajorPivot = false;
                }
                else
                {
                    this.vm.SelectedMaintenanceRepair.IsMajorPivot = true;
                }

            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.vm != null)
            {
                _snapShotsPopup = new SnapshotsViewer();
                var item = e.ClickedItem as ImageCapture;
                _snapShotsPopup.Open(this.vm.SelectedMaintenanceRepair, item.guid);
            }
        }

    }
}
