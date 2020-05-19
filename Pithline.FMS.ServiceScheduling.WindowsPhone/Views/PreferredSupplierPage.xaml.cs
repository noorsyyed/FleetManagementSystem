using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic;
using Pithline.FMS.ServiceScheduling.UILogic.Portable;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.ServiceScheduling.WindowsPhone.Views
{
    public sealed partial class PreferredSupplierPage : VisualStateAwarePage
    {
        SearchSupplierDialog sp;
        DetailsDialog moreInfo;
        public PreferredSupplierPage()
        {
            this.InitializeComponent();
            this.Loaded += PreferredSupplierPage_Loaded;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (sp != null)
            {
                sp.Hide();
            }
            if (moreInfo != null)
            {
                moreInfo.Hide();
            }
        }
        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (sp != null)
            {
                sp.Hide();
            }
            if (moreInfo != null)
            {
                moreInfo.Hide();
            }
        }

        void PreferredSupplierPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.vm = this.DataContext as PreferredSupplierPageViewModel;
        }
        public PreferredSupplierPageViewModel vm { get; set; }
        async private void More_Click(object sender, RoutedEventArgs e)
        {
            moreInfo = new DetailsDialog();
            this.vm = this.DataContext as PreferredSupplierPageViewModel;
            moreInfo.DataContext = vm.SelectedTask;
            await moreInfo.ShowAsync();
        }

        private void filter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (!String.IsNullOrEmpty(text))
            {
                ObservableCollection<BusinessLogic.Portable.SSModels.Supplier> filterResult = new ObservableCollection<BusinessLogic.Portable.SSModels.Supplier>();
                foreach (var task in PersistentData.Instance.PoolofSupplier)
                {
                    if (task.SupplierName.ToLower().Contains(text.ToLower()))
                    {
                        filterResult.Add(task);
                    }
                }
                ((PreferredSupplierPageViewModel)this.DataContext).PoolofSupplier = filterResult;
            }
            else
            {
                ((PreferredSupplierPageViewModel)this.DataContext).PoolofSupplier = PersistentData.Instance.PoolofSupplier;

            }
        }

        async private void filter_Click(object sender, RoutedEventArgs e)
        {

            if (vm != null)
            {
                sp = new SearchSupplierDialog(vm._locationService, vm._eventAggregator, vm._supplierService);
                await sp.ShowAsync();
            }
        }
    }

}
