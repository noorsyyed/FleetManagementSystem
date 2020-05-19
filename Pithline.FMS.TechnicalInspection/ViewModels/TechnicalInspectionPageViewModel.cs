using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.TI;
using Pithline.FMS.TechnicalInspection.UILogic;
using Pithline.FMS.TechnicalInspection.UILogic.AifServices;
using Pithline.FMS.TechnicalInspection.UILogic.Events;
using Pithline.FMS.TechnicalInspection.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using Syncfusion.UI.Xaml.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Pithline.FMS.TechnicalInspection.Common;

namespace Pithline.FMS.TechnicalInspection.ViewModels
{
    public class TechnicalInspectionPageViewModel : BaseViewModel
    {
        private Pithline.FMS.BusinessLogic.TITask _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public TechnicalInspectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                this._eventAggregator = eventAggregator;
                this.InspectionUserControls = new ObservableCollection<UserControl>();
                this.CustomerDetails = new CustomerDetails();
                this.MaintenanceRepairList = new ObservableCollection<MaintenanceRepair>();
                this.Model = new TIData();

                


                this.CompleteCommand = new DelegateCommand(async () =>
                {
                    try
                    {
                        this.IsBusy = true;
                        AppSettings.Instance.IsSynchronizing = 1;
                        this._task.Status = Pithline.FMS.BusinessLogic.Helpers.TaskStatus.Completed;
                        await SqliteHelper.Storage.UpdateSingleRecordAsync(this._task);
                        var model = ((TIData)this.Model);
                        model.ShouldSave = true;
                        model.VehicleInsRecID = _task.CaseServiceRecID;
                        var tiDataTable = await SqliteHelper.Storage.LoadTableAsync<TIData>();
                        if (tiDataTable.Any(x => x.VehicleInsRecID == model.VehicleInsRecID))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<TIData>(model);
                        }
                        else
                        {
                           await SqliteHelper.Storage.InsertSingleRecordAsync<TIData>(model); 
                        }
                        TIServiceHelper.Instance.Synchronize();
                        // this.SaveCurrentUIDataAsync(currentModel);
                        _navigationService.Navigate("Main", null);

                         //await TIServiceHelper.Instance.UpdateTaskStatusAsync();
                        this.IsBusy = false;
                        _eventAggregator.GetEvent<TITaskFetchedEvent>().Publish(this._task);
                    }
                    catch (Exception ex)
                    {
                        this.IsBusy = false;
                        AppSettings.Instance.IsSynchronizing = 0;
                        AppSettings.Instance.ErrorMessage = ex.Message;
                    }
                });

                this._eventAggregator.GetEvent<SignChangedEvent>().Subscribe(p =>
                {
                    CompleteCommand.RaiseCanExecuteChanged();
                });

                this._eventAggregator.GetEvent<ErrorsRaisedEvent>().Subscribe((errors) =>
                {
                    Errors = errors;
                    OnPropertyChanged("Errors");
                    ShowValidationSummary = true;
                    OnPropertyChanged("ShowValidationSummary");
                }, ThreadOption.UIThread);

               

            }
            catch (Exception)
            {
                throw;
            }

        }


        private void LoadAppointments()
        {
            var startTime = new DateTime(this._task.ConfirmedDate.Year, this._task.ConfirmedDate.Month, this._task.ConfirmedDate.Day, this._task.ConfirmedTime.Hour, this._task.ConfirmedTime.Minute,
                                this._task.ConfirmedTime.Second);
            this.CustomerDetails.Appointments = new ScheduleAppointmentCollection
            {
                new ScheduleAppointment(){
                    Subject = this._task.CaseNumber,                    
                    Location =this._task.Address,
                    StartTime = startTime,
                    EndTime = startTime.AddHours(1),
                    ReadOnly = true,
                    AppointmentBackground = new SolidColorBrush(Colors.Crimson),                   
                    Status = new ScheduleAppointmentStatus{Status = this._task.Status,Brush = new SolidColorBrush(Colors.Chocolate)}
                },
                               
            };
        }


        async private System.Threading.Tasks.Task LoadFromDbAsync(long CaseServiceRecId)
        {
            try
            {
                var maintenanceRepairdata = await (SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>());


                foreach (var item in maintenanceRepairdata.Where(x=>x.CaseServiceRecId == _task.CaseServiceRecID))
                {
                    this.MaintenanceRepairList.Add(item);
                }

                TIData viBaseObject = (TIData)this.Model;
                viBaseObject.LoadSnapshotsFromDb();
                //  PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
                viBaseObject.ShouldSave = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long CaseServiceRecId)
        {

            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<TIData>(x => x.VehicleInsRecID == CaseServiceRecId);
            if (this.Model == null)
            {
                this.Model = new TIData();
            }
            await LoadFromDbAsync(CaseServiceRecId);
        }

        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                this.InspectionHistList = new ObservableCollection<InspectionHistory>{
                new InspectionHistory{InspectionResult=new List<string>{"Engine and brake oil replacement","Wheel alignment"},CustomerId="1",InspectedBy="Jon Tabor",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle coolant replacement","Few dent repairs"},CustomerId="1",InspectedBy="Robert Green",InspectedOn = DateTime.Now},
                new InspectionHistory{InspectionResult=new List<string>{"Vehicle is in perfect condition"},CustomerId="1",InspectedBy="Christopher",InspectedOn = DateTime.Now},
            };
                _task = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.TITask>(navigationParameter.ToString());
                App.Task = _task;

                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = _task.CaseNumber;
                LoadAppointments();
                 GetCustomerDetailsAsync();

                _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe( b =>
                {
                     GetCustomerDetailsAsync();
                });

                await this.LoadModelFromDbAsync(this._task.CaseServiceRecID);

            }
            catch (Exception)
            {

                throw;
            }
        }

        private UserControl frameContent;

        public UserControl FrameContent
        {
            get { return frameContent; }
            set { SetProperty(ref frameContent, value); }
        }

        private DelegateCommand completeCommand;

        public DelegateCommand CompleteCommand
        {
            get { return completeCommand; }
            set { SetProperty(ref completeCommand, value); }
        }
        private bool isCommandBarOpen;
        [RestorableState]
        public bool IsCommandBarOpen
        {
            get { return isCommandBarOpen; }
            set { SetProperty(ref isCommandBarOpen, value); }
        }


        private ObservableCollection<MaintenanceRepair> maintenanceRepairList;
        public ObservableCollection<MaintenanceRepair> MaintenanceRepairList
        {
            get { return maintenanceRepairList; }
            set { SetProperty(ref maintenanceRepairList, value); }
        }


        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private ObservableCollection<UserControl> inpectionUserControls;
        public ObservableCollection<UserControl> InspectionUserControls
        {
            get { return inpectionUserControls; }
            set { SetProperty(ref inpectionUserControls, value); }
        }

        private ObservableCollection<InspectionHistory> inspectionHistList;
        public ObservableCollection<InspectionHistory> InspectionHistList
        {
            get { return inspectionHistList; }
            set { SetProperty(ref inspectionHistList, value); }
        }

        private CustomerDetails customerDetails;

        public CustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private Customer customer;
        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
        }

        private bool showValidationSummary;
        [RestorableState]
        public bool ShowValidationSummary
        {
            get { return showValidationSummary; }
            set { SetProperty(ref showValidationSummary, value); }
        }

        private ObservableCollection<ValidationError> errors;
        public ObservableCollection<ValidationError> Errors
        {
            get { return errors; }
            set { SetProperty(ref errors, value); }
        }

        private void GetCustomerDetailsAsync()
        {
            try
            {
                if (this._task != null)
                {
                    //this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                    //if (this.Customer == null)
                    //{
                    //    AppSettings.Instance.IsSyncingCustDetails = 1;

                    //}
                    //else
                    //{
                        AppSettings.Instance.IsSyncingCustDetails = 0;
                        this.CustomerDetails.ContactNumber = this._task.ContactNumber;
                        this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                        this.CustomerDetails.Status = this._task.Status;
                        this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                        this.CustomerDetails.Address = this._task.Address;
                        this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                        this.CustomerDetails.CustomerName = this._task.CustomerName;
                        this.CustomerDetails.ContactName = this._task.ContactName;
                        this.CustomerDetails.CategoryType = this._task.CaseCategory;
                        this.CustomerDetails.EmailId = this._task.Email;

                        this.CustomerDetails.Appointments =PersistentData.Instance.CustomerDetails.Appointments;
                    //}
                }
            }
            catch (Exception)
            {
                throw;
            }

        }


        async private void SaveCurrentUIDataAsync(Object model)
        {
            try
            {
                if (this._task != null)
                {
                    var m = (BaseModel)model;
                    var successFlag = 0;
                    if (m.ShouldSave)
                    {
                        var baseModel = await (model as BaseModel).GetDataAsync(this._task.VehicleInsRecId);


                        if (baseModel != null)
                        {
                            successFlag = await SqliteHelper.Storage.UpdateSingleRecordAsync(m);
                        }
                        else
                        {
                            m.VehicleInsRecID = this._task.VehicleInsRecId;
                            successFlag = await SqliteHelper.Storage.InsertSingleRecordAsync(m);
                        }
                    }

                    if (successFlag != 0)
                    {
                        m.ShouldSave = false;
                        await TIServiceHelper.Instance.SyncFromSvcAsync(m);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
