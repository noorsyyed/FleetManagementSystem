using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Pithline.FMS.ServiceScheduling.UILogic
{
    public class AppSettings : BindableBase
    {
        private static readonly AppSettings _instance = new AppSettings();
        public AppSettings()
        {
            this.ClearErrorMessageCommand = new DelegateCommand(() =>
                {
                    ErrorMessage = string.Empty;
                });
        }
        public static AppSettings Instance { get { return _instance; } }

        public ICommand ClearErrorMessageCommand { get; set; }

        private int isSyncingCustDetails;

        public int IsSyncingCustDetails
        {
            get { return isSyncingCustDetails; }
            set { SetProperty(ref isSyncingCustDetails, value); }
        }

        private int isSyncingVehDetails;

        public int IsSyncingVehDetails
        {
            get { return isSyncingVehDetails; }
            set { SetProperty(ref isSyncingVehDetails, value); }
        }


        private int isSynchronizing;

        public int IsSynchronizing
        {
            get { return isSynchronizing; }
            set { SetProperty(ref isSynchronizing, value); }
        }

        private bool synced;
        public bool Synced
        {
            get { return synced; }
            set { SetProperty(ref synced, value); }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }
    }
}
