using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.TechnicalInspection.ViewModels;
using Pithline.FMS.TechnicalInspection.Views;
using Pithline.FMS.TechnicalInspection.UILogic.AifServices;
using Pithline.FMS.TechnicalInspection.UILogic.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Pithline.FMS.BusinessLogic.TI;
using Pithline.FMS.TechnicalInspection.UILogic;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Pithline.FMS.TechnicalInspection
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase, IDisposable
    {
        public static Pithline.FMS.BusinessLogic.TITask Task { get; set; }
        private readonly IUnityContainer _container = new UnityContainer();
        public IEventAggregator EventAggregator { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.UnhandledException += App_UnhandledException;
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {

            base.OnRegisterKnownTypesForSerialization();
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.UserInfo));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.TITask));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Customer));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.LoggedInUser));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.ImageCapture));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<ImageCapture>));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Enums.CaseTypeEnum));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Helpers.TaskStatus));
            SessionStateService.RegisterKnownType(typeof(Syncfusion.UI.Xaml.Schedule.ScheduleAppointmentCollection));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Enums.VehicleTypeEnum));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.CustomerDetails));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<Pithline.FMS.BusinessLogic.TITask>));
            SessionStateService.RegisterKnownType(typeof(LogonResult));
            SessionStateService.RegisterKnownType(typeof(TIData));
            SessionStateService.RegisterKnownType(typeof(MaintenanceRepair));
            SessionStateService.RegisterKnownType(typeof(List<string>));

        }

        async protected override System.Threading.Tasks.Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {

            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\Pithline.FMSmobility.sqlite") as StorageFile;
            if (db == null)
            {
                var packDb = await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\Pithline.FMSmobility.sqlite");
                // var packDb = await sqliteDBFolder.GetFileAsync("Pithline.FMSmobility.sqlite");
                var destinationFolder = await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB", CreationCollisionOption.ReplaceExisting);
                await packDb.CopyAsync(destinationFolder);
            }
            SqliteHelper.Storage.ConnectionDatabaseAsync();

            var accountService = _container.Resolve<IAccountService>();
            var cred = accountService.VerifyUserCredentialsAsync();
            if (cred != null && ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.UserInfo))
            {
                //string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                //ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                TIServiceHelper.Instance.ConnectAsync(cred.Item1, cred.Item2, EventAggregator);
                NavigationService.Navigate("Main", string.Empty);
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();


        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            try
            {
                base.OnInitialize(args);
                EventAggregator = new EventAggregator();
                _container.RegisterInstance(SessionStateService);
                _container.RegisterInstance(NavigationService);
                _container.RegisterInstance(EventAggregator);


                _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());

                ViewModelLocator.Register(typeof(TechnicalInspectionPage).ToString(), () => new TechnicalInspectionPageViewModel(NavigationService, EventAggregator));

                ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pithline.FMS.TechnicalInspection.UILogic.ViewModels.{0}ViewModel,Pithline.FMS.TechnicalInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                    return Type.GetType(viewModelTypeName);
                });
                //ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                //    {
                //        var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pithline.FMS.TechnicalInspection.UILogic.ViewModels.Passenger.{0}ViewModel,Pithline.FMS.TechnicalInspection.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                //        return Type.GetType(viewModelTypeName);
                //    });
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }

        }



        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        protected override IList<Windows.UI.ApplicationSettings.SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var accountService = _container.Resolve<IAccountService>();
            if (accountService.SignedInUser != null)
            {
                settingsCommands.Add(new SettingsCommand("resetpassword", "Reset Password", (handler) =>
                            {
                                ResetPasswordPage page = new ResetPasswordPage();
                                page.Show();
                            }));
            }

            settingsCommands.Add(new SettingsCommand("privacypolicy", "Privacy Policy", (handler) =>
                {

                }));
            settingsCommands.Add(new SettingsCommand("help", "Help", (handler) =>
                {

                }));
            // args.Request.ApplicationCommands.Add(command); 

            return settingsCommands;
        }

        public void Dispose()
        {
            this._container.Dispose();
        }

        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            AppSettings.Instance.ErrorMessage = e.Message;
        }

    }
}
