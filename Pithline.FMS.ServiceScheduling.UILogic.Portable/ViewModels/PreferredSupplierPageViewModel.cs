using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.PubSubEvents;
using Windows.Storage;
using Newtonsoft.Json;
using Pithline.FMS.BusinessLogic.Portable;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable
{
    public class PreferredSupplierPageViewModel : ViewModel
    {
        public IEventAggregator _eventAggregator;
        private INavigationService _navigationService;
        public ISupplierService _supplierService;
        public ILocationService _locationService;
        public PreferredSupplierPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ISupplierService supplierService, ILocationService locationService)
        {
            this._navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this.PoolofSupplier = new ObservableCollection<Supplier>();
            this._supplierService = supplierService;
            this._locationService = locationService;

            this.NextPageCommand = DelegateCommand.FromAsyncHandler(
         async () =>
         {
             try
             {
                 if (this.SelectedSupplier != null)
                 {
                     this.TaskProgressBar = Visibility.Visible;
                     var supplier = new SupplierSelection() { CaseNumber = this.SelectedTask.CaseNumber, CaseServiceRecID = this.SelectedTask.CaseServiceRecID, SelectedSupplier = this.SelectedSupplier };
                     var response = await this._supplierService.InsertSelectedSupplierAsync(supplier, this.UserInfo);
                     if (response)
                     {
                         navigationService.Navigate("SubmittedDetail", string.Empty);
                     }
                 }

             }
             catch (Exception ex)
             {
                 this.TaskProgressBar = Visibility.Collapsed;
             }
             finally
             {
             }
         },

          () => { return this.SelectedSupplier != null; });


            _eventAggregator.GetEvent<SupplierFilterEvent>().Subscribe(poolofSupplier =>
            {
                this.PoolofSupplier = poolofSupplier;
            });

        }
      
        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.TaskProgressBar = Visibility.Visible;

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SELECTEDTASK))
                {
                    this.SelectedTask = JsonConvert.DeserializeObject<Pithline.FMS.BusinessLogic.Portable.SSModels.Task>(ApplicationData.Current.RoamingSettings.Values[Constants.SELECTEDTASK].ToString());
                }

                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                if (PersistentData.Instance.PoolofSupplier!= null)
                {
                    this.PoolofSupplier = PersistentData.Instance.PoolofSupplier;
                }
                this.PoolofSupplier = await this._supplierService.GetSuppliersByClassAsync(this.SelectedTask.VehicleClassId, this.UserInfo);
                PersistentData.Instance.PoolofSupplier = this.PoolofSupplier;
                this.TaskProgressBar = Visibility.Collapsed;
            }
            catch (Exception)
            {
                this.TaskProgressBar = Visibility.Collapsed;
            }
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

        private Visibility taskProgressBar;
        public Visibility TaskProgressBar
        {
            get { return taskProgressBar; }
            set
            {
                SetProperty(ref taskProgressBar, value);
            }
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
        public UserInfo UserInfo { get; set; }
        public DelegateCommand NextPageCommand { get; private set; }

        public Pithline.FMS.BusinessLogic.Portable.SSModels.Task SelectedTask { get; set; }
    }
}
