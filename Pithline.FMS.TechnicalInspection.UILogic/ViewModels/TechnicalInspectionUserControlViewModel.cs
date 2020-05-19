using Eqstra.BusinessLogic;
using Eqstra.BusinessLogic.Base;
using Eqstra.BusinessLogic.Common;
using Eqstra.BusinessLogic.Helpers;
using Eqstra.BusinessLogic.TI;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace Eqstra.TechnicalInspection.UILogic.ViewModels
{
    class TechnicalInspectionUserControlViewModel : BaseViewModel
    {
        public TechnicalInspectionUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.TechnicalInspList = new ObservableCollection<MaintenanceRepair>();
            LoadModelFromDbAsync(long.Parse(ApplicationData.Current.LocalSettings.Values["CaseServiceRecID"].ToString()));
        }

        private void loadDataFromDb(long CaseServiceRecId)
        {
            try
            {
                var maintenanceRepairdata = (SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>()).Result;

                if (maintenanceRepairdata != null && maintenanceRepairdata.Any())
                {
                    foreach (var item in maintenanceRepairdata)
                    {
                        this.TechnicalInspList.Add(item);
                    }
                }
                TechnicalInsp viBaseObject = (TechnicalInsp)this.Model;
                //viBaseObject.LoadSnapshotsFromDb();
                PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
                viBaseObject.ShouldSave = false;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long CaseServiceRecId)
        {

            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<TechnicalInsp>(x => x.CaseServiceRecID == CaseServiceRecId);
            if (this.Model == null)
            {
                this.Model = new TechnicalInsp();
            }
            loadDataFromDb(CaseServiceRecId);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

        }

        private ObservableCollection<MaintenanceRepair> technicalInspList;
        public ObservableCollection<MaintenanceRepair> TechnicalInspList
        {
            get { return technicalInspList; }
            set { SetProperty(ref technicalInspList, value); }
        }

    }
}
