using Microsoft.Practices.Prism.StoreApps;

namespace Pithline.FMS.DocumentDelivery.UILogic
{
    public class AppSettings : BindableBase
    {
        private static readonly AppSettings _instance = new AppSettings();
        public static AppSettings Instance { get { return _instance; } }

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
