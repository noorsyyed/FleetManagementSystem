using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using System.Linq;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using System;
using System.Runtime.CompilerServices;
namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class TTyreConditionUserControlViewModel : BaseViewModel
    {
        public TTyreConditionUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new TTyreCond();
            this.PoolOfTyreCondions = new ObservableCollection<TTyreCond>();

            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion1" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion2" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion3" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion4" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion5" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion6" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion7" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion8" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion9" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion10" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion11" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion12" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion13" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion14" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion15" });
            PoolOfTyreCondions.Add(new TTyreCond { Position = "Postion16" });

            this.ChangedCommand = new DelegateCommand(() =>
            {
                this.ShouldSave= PropertyHistory.Instance.IsPropertiesOriginalValuesChanged(this.PoolOfTyreCondions);

            });
        }

      

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }


        public async System.Threading.Tasks.Task SaveTrailerTyreConditions(long vehicleInsRecId)
        {
            try
            {
                var successFlag = 0;
                foreach (var tyre in PoolOfTyreCondions)
                {

                    if (tyre.ShouldSave)
                    {
                        var tyreList = (await SqliteHelper.Storage.LoadTableAsync<TTyreCond>()).Where(x => x.VehicleInsRecID == vehicleInsRecId);
                        if (tyreList.Any(a => a.TyreCondID == tyre.TyreCondID))
                        {
                            successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(tyre);
                        }
                        else
                        {
                            tyre.VehicleInsRecID = vehicleInsRecId;
                            successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(tyre);
                        }
                    }

                }
                if (successFlag != 0)
                {
                    await VIServiceHelper.Instance.SyncFromSvcAsync((TTyreCond)this.Model);
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        private void loadDataFromDb(long vehicleInsRecID)
        {
            try
            {
                var tyredata = (SqliteHelper.Storage.LoadTableAsync<TTyreCond>()).Result;

                if (tyredata != null && tyredata.Any())
                {
                    foreach (var item in tyredata)
                    {
                        if (item.VehicleInsRecID == vehicleInsRecID)
                        {
                            var itemToRemove = this.PoolOfTyreCondions.Where(w => w.Position == item.Position).FirstOrDefault();
                            this.PoolOfTyreCondions.Insert(this.PoolOfTyreCondions.IndexOf(itemToRemove), item);
                            this.PoolOfTyreCondions.Remove(itemToRemove);

                        }

                    }
                }
                BaseModel viBaseObject = (TTyreCond)this.Model;
                viBaseObject.VehicleInsRecID = vehicleInsRecID;
                viBaseObject.LoadSnapshotsFromDb();
                PropertyHistory.Instance.SetPropertyHistory(this.PoolOfTyreCondions);
                viBaseObject.ShouldSave = false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            loadDataFromDb(vehicleInsRecID);
        }

        private ObservableCollection<TTyreCond> poolOfTyreCondions;

        public ObservableCollection<TTyreCond> PoolOfTyreCondions
        {
            get { return poolOfTyreCondions; }
            set
            {
                SetProperty(ref poolOfTyreCondions, value);
            }
        }

        private bool shouldSave;

        public bool ShouldSave
        {
            get { return shouldSave; }
            set { SetProperty(ref shouldSave, value); }
        }
        
        public DelegateCommand ChangedCommand { get; set; }

    }
}
