using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Commercial;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.VehicleInspection.UILogic;
using Pithline.FMS.VehicleInspection.UILogic.AifServices;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Pithline.FMS.VehicleInspection.UILogic.ViewModels;
using Pithline.FMS.VehicleInspection.Views;
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

namespace Pithline.FMS.VehicleInspection.ViewModels
{
    public class VehicleInspectionPageViewModel : BaseViewModel
    {
        private Pithline.FMS.BusinessLogic.Task _task;
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;

        public VehicleInspectionPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
            : base(navigationService)
        {
            try
            {
                _navigationService = navigationService;
                this._eventAggregator = eventAggregator;
                this.InspectionUserControls = new ObservableCollection<UserControl>();
                this.CustomerDetails = new CustomerDetails();

                this.PrevViewStack = new Stack<UserControl>();
                // LoadDemoAppointments();

                this.CompleteCommand = new DelegateCommand(async () =>
                {
                    this.IsBusy = true;
                    this._task.ProcessStep = ProcessStep.AcceptInspection;
                    this._task.ShouldSync = true;
                    this._task.Status = Pithline.FMS.BusinessLogic.Helpers.TaskStatus.AwaitDamageConfirmation;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(this._task);
                    var currentViewModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext);
                    var currentModel = currentViewModel.Model;

                    if (currentViewModel is TTyreConditionUserControlViewModel)
                    {
                        await ((TTyreConditionUserControlViewModel)currentViewModel).SaveTrailerTyreConditions(this._task.VehicleInsRecId);
                    }
                    else
                    {
                        this.SaveCurrentUIDataAsync(currentModel);
                    }
                    _navigationService.Navigate("Main", null);

                    await VIServiceHelper.Instance.UpdateTaskStatusAsync();
                    this.IsBusy = false;
                }, () =>
                {
                    var vm = ((BaseViewModel)this.NextViewStack.Peek().DataContext);
                    if (vm is InspectionProofUserControlViewModel)
                    {
                        return (this.NextViewStack.Count == 1) && (((InspectionProofUserControlViewModel)vm).CustSignature != null) && (((InspectionProofUserControlViewModel)vm).PithlineRepSignature != null);
                    }
                    else if (vm is CPOIUserControlViewModel)
                    {
                        return (this.NextViewStack.Count == 1) && (((CPOIUserControlViewModel)vm).CustSignature != null) && (((CPOIUserControlViewModel)vm).PithlineRepSignature != null);
                    }
                    else
                    {
                        return (this.NextViewStack.Count == 1);
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

                this.NextCommand = new DelegateCommand(async () =>
                {
                    //this.IsCommandBarOpen = false;
                    //this.IsFlyoutOpen = true;
                    ShowValidationSummary = false;
                    var currentViewModel = (BaseViewModel)this.NextViewStack.Peek().DataContext;
                    var currentModel = currentViewModel.Model as BaseModel;

                    if (currentModel.ValidateModel() && await currentModel.VehicleDetailsImagesValidate())
                    {

                        this.PrevViewStack.Push(this.NextViewStack.Pop());
                        if (currentViewModel is TTyreConditionUserControlViewModel)
                        {
                            await ((TTyreConditionUserControlViewModel)currentViewModel).SaveTrailerTyreConditions(this._task.VehicleInsRecId);
                        }
                        else
                        {
                            this.SaveCurrentUIDataAsync(currentModel);
                        }

                        this.FrameContent = this.NextViewStack.Peek();
                        CompleteCommand.RaiseCanExecuteChanged();
                        NextCommand.RaiseCanExecuteChanged();
                        PreviousCommand.RaiseCanExecuteChanged();
                        if (this.NextViewStack.FirstOrDefault() != null)
                        {
                            BaseViewModel nextViewModel = this.NextViewStack.FirstOrDefault().DataContext as BaseViewModel;
                            await nextViewModel.LoadModelFromDbAsync(this._task.VehicleInsRecId);
                        }
                    }
                    else
                    {
                        Errors = currentModel.Errors;
                        OnPropertyChanged("Errors");
                        ShowValidationSummary = true;
                    }

                }, () =>
                {
                    return this.NextViewStack.Count > 1;
                });


                this.PreviousCommand = new DelegateCommand(async () =>
                {
                    this.IsCommandBarOpen = false;
                    ShowValidationSummary = false;
                    var currentViewModel = ((BaseViewModel)this.NextViewStack.Peek().DataContext);
                    var currentModel = currentViewModel.Model as BaseModel;

                    if (currentModel is PInspectionProof)
                    {
                        ((InspectionProofUserControlViewModel)this.NextViewStack.Peek().DataContext).CustSignature = null;
                        ((InspectionProofUserControlViewModel)this.NextViewStack.Peek().DataContext).PithlineRepSignature = null;
                        SetFrameContent();
                    }
                    else if (currentModel is CPOI)
                    {
                        ((CPOIUserControlViewModel)this.NextViewStack.Peek().DataContext).CustSignature = null;
                        ((CPOIUserControlViewModel)this.NextViewStack.Peek().DataContext).PithlineRepSignature = null;
                        SetFrameContent();
                    }

                    else
                    {
                        if (currentModel.ValidateModel())
                        {
                            SetFrameContent();


                            if (currentViewModel is TTyreConditionUserControlViewModel)
                            {
                                await ((TTyreConditionUserControlViewModel)currentViewModel).SaveTrailerTyreConditions(this._task.VehicleInsRecId);
                            }
                            else
                            {
                                this.SaveCurrentUIDataAsync(currentModel);
                            }

                            if (this.PrevViewStack.FirstOrDefault() != null)
                            {
                                BaseViewModel nextViewModel = this.PrevViewStack.FirstOrDefault().DataContext as BaseViewModel;
                                await nextViewModel.LoadModelFromDbAsync(this._task.VehicleInsRecId);
                            }
                        }
                        else
                        {
                            Errors = currentModel.Errors;
                            OnPropertyChanged("Errors");
                            ShowValidationSummary = true;
                        }
                    }

                }, () =>
                {
                    return this.PrevViewStack.Count > 0;
                });

            }
            catch (Exception)
            {
                throw;
            }

        }

        private void SetFrameContent()
        {
            var item = this.PrevViewStack.Pop();
            this.FrameContent = item;
            this.NextViewStack.Push(item);
            CompleteCommand.RaiseCanExecuteChanged();
            PreviousCommand.RaiseCanExecuteChanged();
            NextCommand.RaiseCanExecuteChanged();
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
                _task = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Task>(navigationParameter.ToString());
                App.Task = _task;
                StampPersistData.Instance.Task = _task;
                //var vt = await SqliteHelper.Storage.LoadTableAsync<Vehicle>();
                ApplicationData.Current.LocalSettings.Values["CaseNumber"] = _task.CaseNumber;
                LoadAppointments();
                await GetCustomerDetailsAsync();
                if (_task.VehicleType == BusinessLogic.Enums.VehicleTypeEnum.Passenger)
                {
                    this.InspectionUserControls.Add(new PVehicleDetailsUserControl());
                    this.InspectionUserControls.Add(new TrimIntUserControl());
                    this.InspectionUserControls.Add(new BodyworkUserControl());
                    this.InspectionUserControls.Add(new GlassUserControl());
                    this.InspectionUserControls.Add(new AccessoriesUserControl());
                    this.InspectionUserControls.Add(new TyreConditionUserControl());
                    this.InspectionUserControls.Add(new MechanicalCondUserControl());
                    this.InspectionUserControls.Add(new InspectionProofUserControl());
                }
                else if (_task.VehicleType == BusinessLogic.Enums.VehicleTypeEnum.Commercial)
                {
                    this.InspectionUserControls.Add(new CVehicleDetailsUserControl());
                    this.InspectionUserControls.Add(new CabTrimInterUserControl());
                    this.InspectionUserControls.Add(new ChassisBodyUserControl());
                    this.InspectionUserControls.Add(new CGlassUserControl());
                    this.InspectionUserControls.Add(new CAccessoriesUserControl());
                    this.InspectionUserControls.Add(new CTyresUserControl());
                    this.InspectionUserControls.Add(new CMechanicalCondUserControl());
                    this.InspectionUserControls.Add(new CPOIUserControl());
                }
                else
                {
                    this.InspectionUserControls.Add(new TVehicleDetailsUserControl());
                    this.InspectionUserControls.Add(new TAccessoriesUserControl());
                    this.InspectionUserControls.Add(new TChassisBodyUserControl());
                    this.InspectionUserControls.Add(new TGlassUserControl());
                    this.InspectionUserControls.Add(new TTyreConditionUserControl());
                    this.InspectionUserControls.Add(new TMechanicalCondUserControl());
                    this.InspectionUserControls.Add(new TPOIUserControl());
                }

                NextViewStack = new Stack<UserControl>(this.InspectionUserControls.Reverse());
                this.FrameContent = this.inpectionUserControls[0];
                _eventAggregator.GetEvent<CustFetchedEvent>().Subscribe(async b =>
                {
                    await GetCustomerDetailsAsync();
                });
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


        private DelegateCommand nextCommand;

        public DelegateCommand NextCommand
        {
            get { return nextCommand; }
            set
            {
                SetProperty(ref nextCommand, value);
            }
        }

        private DelegateCommand previousCommand;

        public DelegateCommand PreviousCommand
        {
            get { return previousCommand; }
            set { SetProperty(ref previousCommand, value); }
        }

        private Stack<UserControl> nextViewStack;

        public Stack<UserControl> NextViewStack
        {
            get { return nextViewStack; }
            set
            {
                SetProperty(ref nextViewStack, value);
                //NextCommand.RaiseCanExecuteChanged();
            }
        }

        private Stack<UserControl> prevViewStack;
        public Stack<UserControl> PrevViewStack
        {
            get { return prevViewStack; }
            set
            {
                SetProperty(ref prevViewStack, value);
                //PreviousCommand.RaiseCanExecuteChanged();
            }
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

        async private System.Threading.Tasks.Task GetCustomerDetailsAsync()
        {
            try
            {
                if (this._task != null)
                {
                    this.Customer = await SqliteHelper.Storage.GetSingleRecordAsync<Customer>(c => c.Id == this._task.CustomerId);
                    if (this.Customer == null)
                    {
                        AppSettings.Instance.IsSyncingCustDetails = 1;

                    }
                    else
                    {
                        AppSettings.Instance.IsSyncingCustDetails = 0;
                        this.CustomerDetails.ContactNumber = this._task.ContactNumber;
                        this.CustomerDetails.CaseNumber = this._task.CaseNumber;
                        this.CustomerDetails.Status = this._task.Status;
                        this.CustomerDetails.StatusDueDate = this._task.StatusDueDate;
                        this.CustomerDetails.Address = this._task.Address;
                        this.CustomerDetails.AllocatedTo = this._task.AllocatedTo;
                        this.CustomerDetails.CustomerName = this._task.CustomerName;
                        this.CustomerDetails.ContactName = this._task.ContactName;
                        this.CustomerDetails.CategoryType = this._task.CategoryType;
                        this.CustomerDetails.EmailId = this.Customer.EmailId;
                    }
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
                        await VIServiceHelper.Instance.SyncFromSvcAsync(m);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This function is to create table Only it should run only once.
        /// </summary>

    }
}
