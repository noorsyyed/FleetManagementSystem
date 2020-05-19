using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Factories;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Globalization;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Pithline.FMS.TechnicalInspection.UILogic;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Pithline.FMS.TechnicalInspection.WindowsPhone
{
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
            Window.Current.Activated += Current_Activated;
        }
        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {

        }

        async protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args != null)
            {
                switch (args.Kind)
                {
                    case ActivationKind.PickFileContinuation:
                        var arguments = (FileOpenPickerContinuationEventArgs)args;
                        StorageFile file = arguments.Files.FirstOrDefault();
                        if (file != null)
                        {
                            await ReadFile(file);
                        }
                        break;
                }
            }
        }

        public async System.Threading.Tasks.Task ReadFile(StorageFile file)
        {
            byte[] fileBytes = null;

            Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture imageCapture;
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

                    imageCapture = new Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture
                          {
                              ImageBitmap = bitmap,
                              ImageData = Convert.ToBase64String(fileBytes),
                              guid = Guid.NewGuid(),
                              RepairId = 0,
                              Component = ""
                          };

                }

            }

            EventAggregator.GetEvent<ImageCaptureTranEvent>().Publish(imageCapture);
        }


        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
        }

        protected override System.Threading.Tasks.Task OnInitializeAsync(IActivatedEventArgs args)
        {

            SessionStateService.RegisterKnownType(typeof(Task));
            SessionStateService.RegisterKnownType(typeof(TITask));
            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(TIData));
            SessionStateService.RegisterKnownType(typeof(MaintenanceRepair));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture));

            EventAggregator = new EventAggregator();

            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);


            //Register Services

            _container.RegisterType<ITaskService, TaskService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IHttpFactory, HttpFactory>(new ContainerControlledLifetimeManager());


            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.ViewModels.{0}ViewModel,Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone, Version=1.0.0.0, Culture=neutral", viewType.Name);

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