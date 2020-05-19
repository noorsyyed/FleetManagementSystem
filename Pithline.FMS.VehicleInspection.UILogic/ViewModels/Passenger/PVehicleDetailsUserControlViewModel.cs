using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.Storage;
using System;
namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class PVehicleDetailsUserControlViewModel : BaseViewModel
    {
        INavigationService _navigationService;
        public PVehicleDetailsUserControlViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _navigationService = navigationService;
            long vehicleInsRecID = long.Parse(ApplicationData.Current.LocalSettings.Values["vehicleInsRecID"].ToString());
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

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DelegateCommand GoToImageMarkupPageCommand { get; set; }


        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PVehicleDetails>(x => x.VehicleInsRecID.Equals(vehicleInsRecID));
            if (this.Model == null)
            {
                AppSettings.Instance.IsSyncingVehDetails = 1;
                this.Model = new PVehicleDetails() { VehicleInsRecID = vehicleInsRecID};
                
                PropertyHistory.Instance.SetPropertyHistory((PVehicleDetails)this.Model);
            }
            else
            {
                AppSettings.Instance.IsSyncingVehDetails = 0;
                BaseModel viBaseObject = (PVehicleDetails)this.Model;
                viBaseObject.LoadSnapshotsFromDb();
                viBaseObject.ShouldSave = false;
                PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
            }

            DataStamp dataStamp = new DataStamp();
            var d = (PVehicleDetails)this.Model;

            var geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();
            if (position != null && position.Coordinate != null)
            {
                dataStamp.Gps = position.Coordinate.Longitude.ToString() + Environment.NewLine + position.Coordinate.Longitude.ToString();
            }
            dataStamp.CaseNo = d.CaseNumber;
            dataStamp.CusName = StampPersistData.Instance.Task.CustomerName;
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
            if (vehicleInsRecID != default(long))
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
