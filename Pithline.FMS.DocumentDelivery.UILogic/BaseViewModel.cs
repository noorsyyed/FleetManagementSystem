using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DocumentDelivery.UILogic
{
    public class BaseViewModel : ViewModel
    {
        INavigationService _navigationService;
        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.GoHomeCommand = new DelegateCommand(() =>
            {
                _navigationService.ClearHistory();
                _navigationService.Navigate("InspectionDetails", string.Empty);
            });
        }
        public DelegateCommand GoHomeCommand { get; set; }
    }
}
