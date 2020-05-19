using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Commercial;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using Windows.Devices.Geolocation;
using Windows.Storage;


namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class CVehicleDetailsUserControlViewModel : BaseViewModel
    {

        INavigationService _navigationService;
        public CVehicleDetailsUserControlViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _navigationService = navigationService;

            long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
            eventAggregator.GetEvent<VehicleFetchedEvent>().Subscribe(async b =>
            {
                await LoadModelFromDbAsync(vehicleInsRecID);
            }, ThreadOption.UIThread);
            LoadModelFromDbAsync(vehicleInsRecID);
            this.GoToImageMarkupPageCommand = new DelegateCommand(() =>
            {
                _navigationService.Navigate("ImageMarkup", this.Model);
            });
        }

        public DelegateCommand GoToImageMarkupPageCommand { get; set; }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<CVehicleDetails>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new CVehicleDetails() { VehicleInsRecID = vehicleInsRecID};
                
                PropertyHistory.Instance.SetPropertyHistory((CVehicleDetails)this.Model);
                AppSettings.Instance.IsSyncingVehDetails = 1;
            }

            else
            {
                AppSettings.Instance.IsSyncingVehDetails = 0;
                BaseModel viBaseObject = (CVehicleDetails)this.Model;
                viBaseObject.LoadSnapshotsFromDb();
                PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
                viBaseObject.ShouldSave = false;
            }

            DataStamp dataStamp = new DataStamp();
            var d=(CVehicleDetails)this.Model;

            var geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();
            if (position != null && position.Coordinate != null)
            {
                dataStamp.Gps = position.Coordinate.Longitude.ToString() + Environment.NewLine + position.Coordinate.Longitude.ToString();
            }
            dataStamp.CaseNo = d.CaseNumber;
            dataStamp.CusName =  StampPersistData.Instance.Task.CustomerName;
            dataStamp.DateOfFirstReg = "";
            dataStamp.InspectorName = StampPersistData.Instance.Task.AllocatedTo;
            dataStamp.KMReading = "";
            dataStamp.Make = d.Make;
            dataStamp.VehRegNo = d.RegistrationNumber;
            StampPersistData.Instance.DataStamp = dataStamp;

        }

        async public override System.Threading.Tasks.Task TakePictureAsync(ImageCapture param,string fieldName)
        {
            await base.TakePictureAsync(param,fieldName);
            long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["VehicleInsRecID"].ToString());
            if (vehicleInsRecID != null)
            {
                var viobj = await (this.Model as BaseModel).GetDataAsync(vehicleInsRecID);
                if (viobj != null)
                {
                    var successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(this.Model);
                }
                else
                {
                    ((BaseModel)this.Model).VehicleInsRecID = vehicleInsRecID;
                    var successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(this.Model);
                }
            }
        }

    }
}
