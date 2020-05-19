using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DocumentDelivery.UILogic.ViewModels
{
    public class SchedulerPageViewModel : ViewModel
    {
        public SchedulerPageViewModel()
        {

        }
        private CDCustomerDetails customerDetails;
        public CDCustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }
        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            this.CustomerDetails = PersistentData.Instance.CustomerDetails;
        }
    }
}
