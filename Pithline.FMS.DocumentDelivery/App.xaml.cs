using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.AifServices;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.Services;
using Pithline.FMS.DocumentDelivery.UILogic.ViewModels;
using Pithline.FMS.DocumentDelivery.Views;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Pithline.FMS.DocumentDelivery
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
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
            this.UnhandledException += App_UnhandledException;
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.UserInfo));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Customer));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.LoggedInUser));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.Helpers.TaskStatus));
            SessionStateService.RegisterKnownType(typeof(Syncfusion.UI.Xaml.Schedule.ScheduleAppointmentCollection));
            SessionStateService.RegisterKnownType(typeof(Pithline.FMS.BusinessLogic.DocumentDelivery.CDCustomerDetails));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<CollectDeliveryTask>));
            SessionStateService.RegisterKnownType(typeof(List<string>));
        }
        async protected override System.Threading.Tasks.Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            var db = await ApplicationData.Current.RoamingFolder.TryGetItemAsync("SQLiteDB\\Pithline.FMSmobility.sqlite") as StorageFile;
            if (db == null)
            {
                var packDb = await Package.Current.InstalledLocation.GetFileAsync("SqliteDB\\Pithline.FMSmobility.sqlite");

                await packDb.CopyAsync(await ApplicationData.Current.RoamingFolder.CreateFolderAsync("SQLiteDB"));
            }
            SqliteHelper.Storage.ConnectionDatabaseAsync();
            var accountService = _container.Resolve<IAccountService>();
            var cred = accountService.VerifyUserCredentialsAsync();

            if (cred != null && ApplicationData.Current.RoamingSettings.Values.ContainsKey(Pithline.FMS.BusinessLogic.Helpers.Constants.UserInfo))
            {
                DDServiceProxyHelper.Instance.ConnectAsync(cred.Item1, cred.Item2, EventAggregator);

                PersistentData.RefreshInstance();//Here only setting data in new instance, and  getting data in every page.
                NavigationService.Navigate("InspectionDetails", string.Empty);
            }
            else
            {
                NavigationService.Navigate("Login", args.Arguments);
            }
            Window.Current.Activate();
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            base.OnInitialize(args);
            EventAggregator = new EventAggregator();


            _container.RegisterInstance(NavigationService);
            _container.RegisterInstance(EventAggregator);
            _container.RegisterInstance(SessionStateService);

            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());

            _container.RegisterType<SettingsFlyout, AddCustomerPage>(new ContainerControlledLifetimeManager());

            //ViewModelLocator.Register(typeof(CollectionOrDeliveryDetailsPage).ToString(), () => new CollectionOrDeliveryDetailsPageViewModel(this.NavigationService, this.EventAggregator, new AddCustomerPage()));
            //ViewModelLocator.Register(typeof(BriefDetailsUserControlViewModel).ToString(), () => new BriefDetailsUserControlViewModel(this.EventAggregator));

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pithline.FMS.DocumentDelivery.UILogic.ViewModels.{0}ViewModel,Pithline.FMS.DocumentDelivery.UILogic,Version 1.0.0.0, Culture=neutral", viewType.Name);
                return Type.GetType(viewModelTypeName);
            });


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

            return settingsCommands;
        }
        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }



        private async System.Threading.Tasks.Task CreateTableAsync()
        {

            await SqliteHelper.Storage.CreateTableAsync<DocumentDeliveryUpdateDetail>();
            //await SqliteHelper.Storage.CreateTableAsync<Document>();
            //var d = new ObservableCollection<Document>
            //  {
            //      new Document{CaseCategoryRecID=123, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=234, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=345, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=456, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=789, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=985, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=741, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=852, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=145, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //      new Document{CaseCategoryRecID=963, CaseNumber = "Case000454",DocumentType  = "License Disc",RegistrationNumber="Registration Number", Make = "Make",Model = "Model",SerialNumber = "Serial Number"},
            //  };
            // await SqliteHelper.Storage.InsertAllAsync<Document>(d);


            //await SqliteHelper.Storage.DropTableAsync<CDDrivingDuration>();
            //await SqliteHelper.Storage.DropTableAsync<Document>();
            //await SqliteHelper.Storage.DropTableAsync<CollectDeliveryTask>();
            //await SqliteHelper.Storage.DropTableAsync<CDCustomerDetails>();

            //await SqliteHelper.Storage.DropTableAsync<CollectedFromData>();
            //await SqliteHelper.Storage.DropTableAsync<Driver>();
            //await SqliteHelper.Storage.DropTableAsync<Courier>();
            //await SqliteHelper.Storage.DropTableAsync<AlternateContactPerson>();
            //await SqliteHelper.Storage.DropTableAsync<CDCustomer>();

            //await SqliteHelper.Storage.DropTableAsync<DocumentCollectDetail>();
            //await SqliteHelper.Storage.DropTableAsync<DocumnetDeliverDetail>();

            //await SqliteHelper.Storage.CreateTableAsync<CDDrivingDuration>();
            //await SqliteHelper.Storage.CreateTableAsync<Document>();
            //await SqliteHelper.Storage.CreateTableAsync<CollectDeliveryTask>();
            //await SqliteHelper.Storage.CreateTableAsync<CDCustomerDetails>();

            //await SqliteHelper.Storage.CreateTableAsync<CollectedFromData>();
            //await SqliteHelper.Storage.CreateTableAsync<Driver>();
            //await SqliteHelper.Storage.CreateTableAsync<Courier>();
            //await SqliteHelper.Storage.CreateTableAsync<AlternateContactPerson>();
            //await SqliteHelper.Storage.CreateTableAsync<CDCustomer>();

            //await SqliteHelper.Storage.CreateTableAsync<DocumentCollectDetail>();
            //await SqliteHelper.Storage.CreateTableAsync<DocumnetDeliverDetail>();

        }
    }
}
