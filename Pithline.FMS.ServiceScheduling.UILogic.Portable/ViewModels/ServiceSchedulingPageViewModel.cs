using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Pithline.WinRT.Components.Controls.WindowsPhone;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable
{
    public class ServiceSchedulingPageViewModel : ViewModel
    {
        public IEventAggregator _eventAggregator;
        public ILocationService _locationService;
        public ISupplierService _supplierService;
        private AddressDialog _addressDialog;
        private ImageViewerPopup _imageViewer;
        private INavigationService _navigationService;
        public IServiceDetailService _serviceDetailService;
        public BusyIndicator _busyIndicator;
        private DetailsDialog moreInfo;
        private ITaskService _taskService;
        private SearchSupplierDialog sp;
        public ServiceSchedulingPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ILocationService locationService, IServiceDetailService serviceDetailService, ISupplierService supplierService, ITaskService taskService)
        {
            this._navigationService = navigationService;
            this._serviceDetailService = serviceDetailService;
            this._taskService = taskService;
            this._eventAggregator = eventAggregator;
            this._locationService = locationService;
            this._supplierService = supplierService;
            this.Model = new ServiceSchedulingDetail();
            _busyIndicator = new BusyIndicator();
            IsEnabledDesType = true;
            this.Address = new BusinessLogic.Portable.SSModels.Address();
            this.applicationTheme = Application.Current.RequestedTheme;
            this.SpBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            this.LtBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            this.DtBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            this.StBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            this.IsLiftRequired = false;
            this.AddVisibility = Visibility.Collapsed;
            BoundWidth = Window.Current.Bounds.Width - 30;
            BoundMinWidth = Window.Current.Bounds.Width - 80;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
           async () =>
           {
               try
               {
                   if (this.Validate())
                   {
                       _busyIndicator.Open("Please wait, Saving ...");

                       this.Model.ServiceDateOption1 = this.Model.ServiceDateOpt1.ToString("MM/dd/yyyy HH:mm");
                       this.Model.ServiceDateOption2 = this.Model.ServiceDateOpt2.ToString("MM/dd/yyyy HH:mm");
                       this.Model.ODOReadingDate = this.Model.ODOReadingDt.ToString("MM/dd/yyyy HH:mm");
                       bool response = await _serviceDetailService.InsertServiceDetailsAsync(this.Model, this.Address, this.UserInfo);
                       if (response)
                       {
                           var caseStatus = await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);
                           var supplier = new SupplierSelection() { CaseNumber = this.SelectedTask.CaseNumber, CaseServiceRecID = this.SelectedTask.CaseServiceRecID, SelectedSupplier = this.SelectedSupplier };
                           var res = await this._supplierService.InsertSelectedSupplierAsync(supplier, this.UserInfo);
                           if (res)
                           {
                               this.SelectedTask.Status = caseStatus.Status;
                               await this._taskService.UpdateStatusListAsync(this.SelectedTask, this.UserInfo);
                               PersistentData.RefreshInstance();
                               navigationService.Navigate("Main", string.Empty);
                           }

                       }
                       _busyIndicator.Close();
                   }
               }
               catch (Exception ex)
               {
                   _busyIndicator.Close();
               }
               finally
               {
               }
           },

            () => { return this.Model != null; });


            this.TakePictureCommand = DelegateCommand<ImageCapture>.FromAsyncHandler(async (param) =>
          {

              FileOpenPicker openPicker = new FileOpenPicker();
              openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
              openPicker.FileTypeFilter.Add(".bmp");
              openPicker.FileTypeFilter.Add(".png");
              openPicker.FileTypeFilter.Add(".jpeg");
              openPicker.FileTypeFilter.Add(".jpg");
              PersistentData.Instance.ServiceSchedulingDetail = this.Model;

              openPicker.PickSingleFileAndContinue();

              this._eventAggregator.GetEvent<ServiceSchedulingDetailEvent>().Subscribe(model =>
              {
                  this.Model = model;
              });
              this._eventAggregator.GetEvent<ImageCaptureEvent>().Subscribe(imageCapture =>
              {
                  this.Model.OdoReadingImageCapture = imageCapture;
              });

          });
            this.OpenImageViewerCommand = new DelegateCommand(
              async () =>
              {

                  if (_imageViewer == null)
                  {
                      _imageViewer = new ImageViewerPopup(this._eventAggregator, this.Model);

                  }
                  else
                  {
                      _imageViewer = null;
                      this._imageViewer = new ImageViewerPopup(this._eventAggregator, this.Model);
                  }

                  _imageViewer.DataContext = this.Model.OdoReadingImageCapture;
                  await _imageViewer.ShowAsync();

              });


            this.VoiceCommand = new DelegateCommand(async () =>
            {
                try
                {
                    SpeechRecognizer recognizer = new SpeechRecognizer();

                    SpeechRecognitionTopicConstraint topicConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "Development");

                    recognizer.Constraints.Add(topicConstraint);
                    await recognizer.CompileConstraintsAsync();

                    var results = await recognizer.RecognizeWithUIAsync();
                    if (results != null & (results.Confidence != SpeechRecognitionConfidence.Rejected))
                    {
                        this.Model.AdditionalWork = results.Text;
                    }
                    else
                    {
                        await new MessageDialog("Sorry, I did not get that.").ShowAsync();
                    }
                }
                catch (Exception)
                {

                }

            });


            this.AddCommand = new DelegateCommand(async () =>
            {
                _addressDialog = new AddressDialog(this._locationService, this._eventAggregator,this.Address);
                this.Model.SelectedDestinationType = new DestinationType();
                await _addressDialog.ShowAsync();
            });

            this.DetailCommand = new DelegateCommand(async () =>
            {
                moreInfo = new DetailsDialog();
                moreInfo.DataContext = this.SelectedTask;
                await moreInfo.ShowAsync();
            });

            this.SupplierFilterCommand = new DelegateCommand(async () =>
            {
                sp = new SearchSupplierDialog(this._locationService, this._eventAggregator, this._supplierService);
                await sp.ShowAsync();

            });

            this._eventAggregator.GetEvent<AddressFilterEvent>().Subscribe((address) =>
            {
                if (address != null)
                {
                    this.Address = address;
                    StringBuilder sb = new StringBuilder();

                    sb.Append(address.Street).Append(",").Append(Environment.NewLine);

                    if ((address.SelectedSuburb != null) && !String.IsNullOrEmpty(address.SelectedSuburb.Name))
                    {
                        sb.Append(address.SelectedSuburb.Name).Append(",").Append(Environment.NewLine);
                    }
                    if (address.SelectedRegion != null)
                    {
                        sb.Append(address.SelectedRegion.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.SelectedCity != null) && !String.IsNullOrEmpty(address.SelectedCity.Name))
                    {
                        sb.Append(address.SelectedCity.Name).Append(",").Append(Environment.NewLine);
                    }
                    if ((address.Selectedprovince != null) && !String.IsNullOrEmpty(address.Selectedprovince.Name))
                    {
                        sb.Append(address.Selectedprovince.Name).Append(",").Append(Environment.NewLine);
                    }

                    if ((address.SelectedCountry != null) && !String.IsNullOrEmpty(address.SelectedCountry.Name))
                    {
                        sb.Append(address.SelectedCountry.Name).Append(",").Append(Environment.NewLine);
                    }

                    sb.Append(address.SelectedZip);


                    this.Model.Address = sb.ToString();
                }
            });

            _eventAggregator.GetEvent<SupplierFilterEvent>().Subscribe(poolofSupplier =>
            {
                this.PoolofSupplier = poolofSupplier;
            });
        }
        private bool Validate()
        {
            Boolean resp = true;
            if (String.IsNullOrEmpty(this.Model.SelectedServiceType))
            {
                this.StBorderBrush = new SolidColorBrush(Colors.Red);
                resp = false;
            }
            else
            {
                this.StBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            }

            if (this.SelectedSupplier == null)
            {
                this.SpBorderBrush = new SolidColorBrush(Colors.Red);
                resp = false;
            }
            else
            {
                this.SpBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
            }

            if (this.IsLiftRequired)
            {
                if (this.Model.SelectedLocationType == null)
                {
                    this.LtBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.LtBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                }
                if (this.Model.SelectedDestinationType == null)
                {
                    this.DtBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.DtBorderBrush = this.applicationTheme == ApplicationTheme.Dark ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Black);
                }
                if (String.IsNullOrEmpty(this.Model.Address))
                {
                    this.AdBorderBrush = new SolidColorBrush(Colors.Red);
                    resp = false;
                }
                else
                {
                    this.AdBorderBrush = null;
                }
            }
            else
            {
                this.Model.SelectedLocationType = null;
                this.Model.SelectedDestinationType = null;
                this.Model.Address=string.Empty;
            }
            return resp;
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            this._busyIndicator.Close();
            base.OnNavigatedFrom(viewModelState, suspending);
            if (moreInfo != null)
            {
                moreInfo.Hide();
            }
            if (sp != null)
            {
                sp.Hide();
            }
        }
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                _busyIndicator.Open("Please wait, loading ...");
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.SSModels.Task>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }

                this.Model = await _serviceDetailService.GetServiceDetailAsync(SelectedTask.CaseNumber, SelectedTask.CaseServiceRecID, SelectedTask.ServiceRecID, this.UserInfo);
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(SelectedTask.VehicleClassId, this.UserInfo);
                if (string.IsNullOrEmpty(this.Model.ODOReadingSnapshot))
                {
                    this.Model.OdoReadingImageCapture = new ImageCapture() { ImageBitmap = new BitmapImage(new Uri("ms-appx:///Assets/odo_meter.png")) };
                }
                else
                {
                    var bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(await this.ConvertToRandomAccessStreamAsync(Convert.FromBase64String(this.Model.ODOReadingSnapshot)));
                    this.Model.OdoReadingImageCapture = new ImageCapture() { ImageBitmap = bitmap };
                }


                if (this.Model == null)
                {
                    this.Model = navigationParameter as ServiceSchedulingDetail;
                }
                if (!String.IsNullOrEmpty(this.Model.ServiceDateOption1))
                {
                    this.Model.ServiceDateOpt1 = DateTime.Parse(this.Model.ServiceDateOption1);
                }
                if (!String.IsNullOrEmpty(this.Model.ServiceDateOption2))
                {
                    this.Model.ServiceDateOpt2 = DateTime.Parse(this.Model.ServiceDateOption2);
                }

                if (!String.IsNullOrEmpty(this.Model.ODOReadingDate))
                {
                    this.Model.ODOReadingDt = DateTime.Parse(this.Model.ODOReadingDate);
                }

                if (this.Model != null)
                {
                    this.IsLiftRequired = this.Model.IsLiftRequired;
                }
                this.Model.CaseNumber = this.SelectedTask.CaseNumber;
                _busyIndicator.Close();
            }
            catch (Exception)
            {
                _busyIndicator.Close();
            }
        }

        private async Task<IRandomAccessStream> ConvertToRandomAccessStreamAsync(byte[] bytes)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            using (var writer = new DataWriter(randomAccessStream))
            {
                writer.WriteBytes(bytes);
                await writer.StoreAsync();
                await writer.FlushAsync();
                writer.DetachStream();
            }
            randomAccessStream.Seek(0);

            return randomAccessStream;
        }
        public UserInfo UserInfo { get; set; }
        private ServiceSchedulingDetail model;
        public ServiceSchedulingDetail Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        private Address address;
        public Address Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        public Pithline.FMS.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand<ImageCapture> TakePictureCommand { get; set; }
        public DelegateCommand OpenImageViewerCommand { get; set; }
        public DelegateCommand SupplierFilterCommand { get; set; }
        public DelegateCommand VoiceCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }

        private Boolean isLiftRequired;
        public Boolean IsLiftRequired
        {
            get { return isLiftRequired; }
            set
            {

                SetProperty(ref isLiftRequired, value);
                if (value)
                {
                    this.IsReqVisibility = Visibility.Visible;
                }
                else
                {
                    this.IsReqVisibility = Visibility.Collapsed;
                }

            }
        }

        private List<Supplier> suppliers;
        public List<Supplier> Suppliers
        {
            get { return suppliers; }
            set { SetProperty(ref suppliers, value); }
        }

        private ObservableCollection<DestinationType> destinationTypes;
        public ObservableCollection<DestinationType> DestinationTypes
        {
            get { return destinationTypes; }
            set { SetProperty(ref destinationTypes, value); }
        }

        private ObservableCollection<Supplier> poolofSupplier;
        public ObservableCollection<Supplier> PoolofSupplier
        {
            get { return poolofSupplier; }
            set
            {
                SetProperty(ref poolofSupplier, value);
            }
        }

        private Visibility isReqVisibility;
        public Visibility IsReqVisibility
        {
            get { return isReqVisibility; }
            set { SetProperty(ref isReqVisibility, value); }
        }

        private Supplier selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                SetProperty(ref selectedSupplier, value);
                this.NextPageCommand.RaiseCanExecuteChanged();
            }
        }

        private Brush stBorderBrush;
        public Brush StBorderBrush
        {
            get { return stBorderBrush; }
            set { SetProperty(ref stBorderBrush, value); }
        }

        private Brush ltBorderBrush;
        public Brush LtBorderBrush
        {
            get { return ltBorderBrush; }
            set { SetProperty(ref ltBorderBrush, value); }
        }


        private Brush dtBorderBrush;
        public Brush DtBorderBrush
        {
            get { return dtBorderBrush; }
            set { SetProperty(ref dtBorderBrush, value); }
        }
        private Brush adBorderBrush;
        public Brush AdBorderBrush
        {
            get { return adBorderBrush; }
            set { SetProperty(ref adBorderBrush, value); }
        }

        private Brush spBorderBrush;

        public Brush SpBorderBrush
        {
            get { return spBorderBrush; }
            set { SetProperty(ref spBorderBrush, value); }
        }

        private bool isEnabledDesType;

        public bool IsEnabledDesType
        {
            get { return isEnabledDesType; }
            set { SetProperty(ref isEnabledDesType, value); }
        }

        private Visibility addVisibility;
        public Visibility AddVisibility
        {
            get { return addVisibility; }
            set { SetProperty(ref addVisibility, value); }
        }
        private double boundWidth;
        public double BoundWidth
        {
            get { return boundWidth; }
            set { SetProperty(ref boundWidth, value); }
        }

        private double boundMinWidth;
        public double BoundMinWidth
        {
            get { return boundMinWidth; }
            set { SetProperty(ref boundMinWidth, value); }
        }
        public ApplicationTheme applicationTheme { get; set; }

        public DelegateCommand DetailCommand { get; set; }
    }
}