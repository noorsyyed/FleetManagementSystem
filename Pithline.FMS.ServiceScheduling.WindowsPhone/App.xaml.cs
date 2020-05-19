using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Factories;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Globalization;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using System.Linq;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Pithline.FMS.ServiceScheduling.WindowsPhone
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : MvvmAppBase
    {
        private readonly IUnityContainer _container = new UnityContainer();
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Light;
            this.UnhandledException += App_UnhandledException;
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        async protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args != null)
            {
                switch (args.Kind)
                {
                    case ActivationKind.PickFileContinuation:
                        var arguments = (FileOpenPickerContinuationEventArgs)args;

                        var serviceSchedulingDetail = PersistentData.Instance.ServiceSchedulingDetail;
                        StorageFile file = arguments.Files.FirstOrDefault();
                        if (file != null)
                        {
                            await ReadFile(file, serviceSchedulingDetail);
                        }
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task ReadFile(StorageFile file, ServiceSchedulingDetail serviceSchedulingDetail)
        {
            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);

                    var bitmap = new BitmapImage();
                    stream.Seek(0);
                    await bitmap.SetSourceAsync(stream);

                    if (serviceSchedulingDetail == null)
                    {
                        serviceSchedulingDetail = new ServiceSchedulingDetail();
                    }
                    serviceSchedulingDetail.OdoReadingImageCapture = new ImageCapture
                        {
                            ImageBitmap = bitmap,
                            ImageBinary = Convert.ToBase64String(fileBytes),
                        };
                    serviceSchedulingDetail.ODOReadingSnapshot = Convert.ToBase64String(fileBytes);
                }
            }

            EventAggregator.GetEvent<ServiceSchedulingDetailEvent>().Publish(serviceSchedulingDetail);
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
        }

        protected override System.Threading.Tasks.Task OnInitializeAsync(IActivatedEventArgs args)
        {
            SessionStateService.RegisterKnownType(typeof(Country));
            SessionStateService.RegisterKnownType(typeof(Province));
            SessionStateService.RegisterKnownType(typeof(City));
            SessionStateService.RegisterKnownType(typeof(Suburb));
            SessionStateService.RegisterKnownType(typeof(Region));

            SessionStateService.RegisterKnownType(typeof(Task));
            SessionStateService.RegisterKnownType(typeof(ServiceSchedulingDetail));
            SessionStateService.RegisterKnownType(typeof(Supplier));
            SessionStateService.RegisterKnownType(typeof(DestinationType));
            SessionStateService.RegisterKnownType(typeof(LocationType));
            SessionStateService.RegisterKnownType(typeof(Address));

            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(ImageCapture));

            EventAggregator = new EventAggregator();


            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);


            //Register Services

            _container.RegisterType<ITaskService, TaskService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISupplierService, SupplierService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IServiceDetailService, ServiceDetailService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocationService, LocationService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHttpFactory, HttpFactory>(new ContainerControlledLifetimeManager());


            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pithline.FMS.ServiceScheduling.UILogic.Portable.{0}ViewModel,Pithline.FMS.ServiceScheduling.UILogic.Portable, Version=1.0.0.0, Culture=neutral", viewType.Name);

                return Type.GetType(viewModelTypeName);
            });
            return base.OnInitializeAsync(args);
        }


        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }


        protected override System.Threading.Tasks.Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.ACCESSTOKEN))
            {
                var accessToken = JsonConvert.DeserializeObject<AccessToken>(ApplicationData.Current.RoamingSettings.Values[Constants.ACCESSTOKEN].ToString());
                if (accessToken.ExpirationDate > DateTime.Now)
                {
                    NavigationService.Navigate("Main", string.Empty);
                }
                else
                {
                    NavigationService.Navigate("Login", args.Arguments);
                }
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();
            return System.Threading.Tasks.Task.FromResult<object>(null);
        }
    }
}