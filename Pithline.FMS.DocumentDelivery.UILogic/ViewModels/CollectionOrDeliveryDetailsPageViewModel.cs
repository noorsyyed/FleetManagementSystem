using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.AifServices;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.DocumentDelivery.UILogic.ViewModels
{
    public class CollectionOrDeliveryDetailsPageViewModel : BaseViewModel
    {
        private INavigationService _navigationService;
        private IEventAggregator _eventAggregator;
        private CollectDeliveryTask _task;
        private SettingsFlyout _addCustomerPage;
        public CollectionOrDeliveryDetailsPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, SettingsFlyout addCustomerPage)
            : base(navigationService)
        {
            _navigationService = navigationService;
            this._eventAggregator = eventAggregator;
            this._addCustomerPage = addCustomerPage;
            this._task = PersistentData.Instance.CollectDeliveryTask;
            this.CustomerDetails = PersistentData.Instance.CustomerDetails;
            this.DocumentList = new ObservableCollection<Document>();
            this.CollectVisibility = Visibility.Collapsed;
            this.CompleteVisibility = Visibility.Collapsed;
            this.DocuDeliveryDetails = new CollectDeliveryDetail();
            this.DocumentCollectDetail = new DocumentCollectDetail();
            this.DocumnetDeliverDetail = new DocumnetDeliverDetail();
            this.AddContactCommand = new DelegateCommand(() =>
            {
                addCustomerPage.ShowIndependent();
            });
            this.CollectCommand = new DelegateCommand(async () =>
            {
                try
                {
                    this.IsBusy = true;

                    switch (this._userInfo.CDUserType)
                    {
                        case CDUserType.Driver:
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                                {
                                    switch (task.ServiceId)
                                    {
                                        case CollectDeliverServices.LICENCEDUP:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;
                                        case CollectDeliverServices.PERMIT:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;

                                        case CollectDeliverServices.POLICECLEARANCE:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;

                                        case CollectDeliverServices.FINESPOB:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;

                                        case CollectDeliverServices.FINESRTA:
                                            task.Status = CDTaskStatus.AwaitDeliveryConfirmation;
                                            task.TaskType = CDTaskType.None;
                                            break;
                                       
                                        case CollectDeliverServices.PNP:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;
                                        case CollectDeliverServices.REQUESTCOF:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;
                                        case CollectDeliverServices.FINESRTC:
                                            task.Status = CDTaskStatus.AwaitDeliveryConfirmation;
                                            task.TaskType = CDTaskType.None;
                                            break;
                                        case CollectDeliverServices.LICENCERENEWAL:
                                            task.Status = CDTaskStatus.AwaitInvoice;
                                            task.TaskType = CDTaskType.None;
                                            break;

                                        default:

                                            task.Status = CDTaskStatus.AwaitDeliveryConfirmation;
                                            task.TaskType = CDTaskType.Delivery;

                                            break;
                                    }



                                    task.IsNotSyncedWithAX = true;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                    await CollectedDocumnetsFromTaskBucket(task, false);
                                }
                            }
                            break;
                        case CDUserType.Courier:
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.AwaitDeliveryConfirmation;
                                    task.TaskType = CDTaskType.Delivery;
                                    task.IsNotSyncedWithAX = true;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                    this.DocuDeliveryDetails.DeliveryDate = this.DocuDeliveryDetails.ReceivedDate;
                                    await DeliveredDocumentsFromTaskBucket(task);
                                }
                            }
                            break;
                        case CDUserType.Customer: foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.Completed;
                                    task.TaskType = CDTaskType.None;
                                    task.IsNotSyncedWithAX = true;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                    await CollectedDocumnetsFromTaskBucket(task, true);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                    this.IsBusy = false;
                    _navigationService.Navigate("InspectionDetails", string.Empty);
                }
                catch (Exception ex)
                {
                    AppSettings.Instance.ErrorMessage = ex.Message;
                    this.IsBusy = false;
                }
            },
            () =>
            {
                return (this.DocumentList.Any(a => a.IsMarked) && ((this.SelectedContact != null && !this.IsAlternateOn) || (this.SelectedAlternateContact != null && this.IsAlternateOn))
                    && this.CRSignature != null && _task.TaskType == CDTaskType.Collect);
            }
            );

            this.CompleteCommand = new DelegateCommand(async () =>
                {
                    try
                    {
                        this.IsBusy = true;

                        if (this._userInfo.CDUserType == CDUserType.Courier)
                        {
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber & a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.AwaitInvoice;
                                    task.TaskType = CDTaskType.None;
                                    task.IsNotSyncedWithAX = true;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);

                                    //await DeliveredDocumentsFromTaskBucket(task);
                                    await UpdateDeliveryDetailForCourierAsync(task);
                                }
                            }
                        }
                        else
                        {
                            foreach (var task in this.SelectedTaskBucket)
                            {
                                if (this.DocumentList.Any(a => a.CaseNumber == task.CaseNumber && a.IsMarked))
                                {
                                    task.Status = CDTaskStatus.Completed;
                                    task.TaskType = CDTaskType.None;
                                    task.IsNotSyncedWithAX = true;
                                    await SqliteHelper.Storage.UpdateSingleRecordAsync(task);
                                    await DeliveredDocumentsFromTaskBucket(task);
                                }
                            }
                        }
                        await DDServiceProxyHelper.Instance.SynchronizeAllAsync();
                        this.IsBusy = false;
                        _navigationService.Navigate("InspectionDetails", string.Empty);

                    }
                    catch (Exception ex)
                    {
                        AppSettings.Instance.ErrorMessage = ex.Message;
                        this.IsBusy = false;
                    }

                }, () =>
                {

                    return (this.DocumentList.Any(a => a.IsMarked) && ((this.SelectedContact != null && !this.IsAlternateOn) || (this.SelectedAlternateContact != null && this.IsAlternateOn))
                        && this.CRSignature != null && _task.TaskType == CDTaskType.Delivery);
                });
            //this.SelectedDocuments = new ObservableCollection<Document>();

            this._eventAggregator.GetEvent<AlternateContactPersonEvent>().Subscribe(async (customerContacts) =>
           {
               this.AlternateContactPersons = await SqliteHelper.Storage.LoadTableAsync<AlternateContactPerson>();
               this._addCustomerPage.Hide();
           });

            this.DocumentsChangedCommand = new DelegateCommand<ObservableCollection<object>>((param) =>
            {
                foreach (var item in this.DocumentList)
                {
                    item.IsMarked = false;
                }
                foreach (var item in param)
                {
                    ((Document)item).IsMarked = true;
                }
                this.CompleteCommand.RaiseCanExecuteChanged();
                this.CollectCommand.RaiseCanExecuteChanged();
            });

        }

        async private System.Threading.Tasks.Task CollectedDocumnetsFromTaskBucket(CollectDeliveryTask task, bool iscollectedByCustomer)
        {
            try
            {
                var collectDetailTable = await SqliteHelper.Storage.LoadTableAsync<DocumentCollectDetail>();
                this.DocumentCollectDetail.Comment = this.DocuDeliveryDetails.Comment;
                this.DocumentCollectDetail.ReceivedDate = this.DocuDeliveryDetails.ReceivedDate;
                this.DocumentCollectDetail.IsColletedByCustomer = iscollectedByCustomer;

                this.DocumentCollectDetail.CollectedAt = this.SelectedCollectedFrom != null ? this.SelectedCollectedFrom.Address : string.Empty;
                this.DocumentCollectDetail.SelectedCollectedFrom = this.SelectedCollectedFrom != null ? this.SelectedCollectedFrom.UserName : string.Empty;

                if (this.IsAlternateOn && SelectedAlternateContact != null)
                {
                    this.DocumentCollectDetail.ReceivedBy = this.SelectedAlternateContact.FullName;
                    this.DocumentCollectDetail.DeliveryPersonName = this.SelectedAlternateContact.FullName;

                    this.DocumentCollectDetail.Email = this.SelectedAlternateContact.Email;
                    this.DocumentCollectDetail.Position = this.SelectedAlternateContact.Position;
                    this.DocumentCollectDetail.Phone = this.SelectedAlternateContact.CellPhone;

                }
                else
                {
                    this.DocumentCollectDetail.ReceivedBy = this.SelectedContact != null ? this.SelectedContact.UserName : string.Empty;
                }

                this.DocumentCollectDetail.CaseNumber = task.CaseNumber;
                this.DocumentCollectDetail.CaseServiceRecId = task.CaseServiceRecID;

                if (collectDetailTable.Any(a => a.CaseNumber == this.DocumentCollectDetail.CaseNumber))
                {
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<DocumentCollectDetail>(this.DocumentCollectDetail);
                }
                else
                {
                    await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentCollectDetail>(this.DocumentCollectDetail);
                }
            }
            catch (Exception ex)
            {

                AppSettings.Instance.ErrorMessage = ex.Message;
            }

        }

        async private System.Threading.Tasks.Task UpdateDeliveryDetailForCourierAsync(CollectDeliveryTask task)
        {
            try
            {
                var deliveryDetailTable = await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryUpdateDetail>();
                var deliveryDetail = new DocumentDeliveryUpdateDetail();
                deliveryDetail.Comment = this.DocuDeliveryDetails.Comment;
                deliveryDetail.DeliveryDate = this.DocuDeliveryDetails.DeliveryDate;

                if (this.IsAlternateOn && SelectedAlternateContact != null)
                {
                    deliveryDetail.ReceivedBy = this.SelectedAlternateContact.FullName;
                    deliveryDetail.DeliveryPersonName = this.SelectedAlternateContact.FullName;
                    deliveryDetail.Email = this.SelectedAlternateContact.Email;
                    deliveryDetail.Position = this.SelectedAlternateContact.Position;
                    deliveryDetail.Phone = this.SelectedAlternateContact.CellPhone;

                }
                else
                {
                    deliveryDetail.DeliveryPersonName = this.SelectedContact != null ? this.SelectedContact.UserName : string.Empty;
                }

                deliveryDetail.CaseNumber = task.CaseNumber;
                deliveryDetail.CaseServiceRecId = task.CaseServiceRecID;

                if (deliveryDetailTable.Any(a => a.CaseNumber == this.DocuDeliveryDetails.CaseNumber))
                {
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<DocumentDeliveryUpdateDetail>(deliveryDetail);
                }
                else
                {
                    await SqliteHelper.Storage.InsertSingleRecordAsync<DocumentDeliveryUpdateDetail>(deliveryDetail);
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        async private System.Threading.Tasks.Task DeliveredDocumentsFromTaskBucket(CollectDeliveryTask task)
        {
            try
            {
                var deliveryDetailTable = await SqliteHelper.Storage.LoadTableAsync<DocumnetDeliverDetail>();
                this.DocumnetDeliverDetail.Comment = this.DocuDeliveryDetails.Comment;
                this.DocumnetDeliverDetail.DeliveryDate = this.DocuDeliveryDetails.DeliveryDate;

                if (this.IsAlternateOn && SelectedAlternateContact != null)
                {
                    this.DocumnetDeliverDetail.ReceivedBy = this.SelectedAlternateContact.FullName;
                    this.DocumnetDeliverDetail.DeliveryPersonName = this.SelectedAlternateContact.FullName;
                    this.DocumnetDeliverDetail.Email = this.SelectedAlternateContact.Email;
                    this.DocumnetDeliverDetail.Position = this.SelectedAlternateContact.Position;
                    this.DocumnetDeliverDetail.Phone = this.SelectedAlternateContact.CellPhone;

                }
                else
                {
                    this.DocumnetDeliverDetail.DeliveryPersonName = this.SelectedContact != null ? this.SelectedContact.UserName : string.Empty;
                }

                this.DocumnetDeliverDetail.CaseNumber = task.CaseNumber;
                this.DocumnetDeliverDetail.CaseServiceRecId = task.CaseServiceRecID;

                if (deliveryDetailTable.Any(a => a.CaseNumber == this.DocuDeliveryDetails.CaseNumber))
                {
                    await SqliteHelper.Storage.UpdateSingleRecordAsync<DocumnetDeliverDetail>(this.DocumnetDeliverDetail);
                }
                else
                {
                    await SqliteHelper.Storage.InsertSingleRecordAsync<DocumnetDeliverDetail>(this.DocumnetDeliverDetail);
                }
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            try
            {
                this.IsBusy = true;
                this.IsDelSignatureDate = Visibility.Collapsed;
                this.IsCollSignatureDate = Visibility.Collapsed;

                if (_userInfo == null)
                {
                    this._userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }


                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                if (_task.TaskType == BusinessLogic.Enums.CDTaskType.Collect)
                {
                    this.ProofTitle = "Proof of Collection";
                    this.AckDialog = string.Empty;
                    this.CDTitle = "Tasks Details";
                    this.CollectVisibility = Visibility.Visible;
                    this.CompleteVisibility = Visibility.Collapsed;
                }
                else
                {
                    this.CompleteVisibility = Visibility.Visible;
                    this.CollectVisibility = Visibility.Collapsed;
                    this.ProofTitle = "Proof of Delivery";
                    this.AckDialog = "I hereby confirm that i have received the following documents";
                    this.CDTitle = "Acknowledgement";
                    this.DocuDeliveryDetails.DeliveredAt = this.CustomerDetails.Address;
                }

                this.SelectedTaskBucket = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(d => d.Status != CDTaskStatus.Completed && d.CustomerId == this._task.CustomerId &&
                    d.ContactPersonAddress == this._task.ContactPersonAddress && d.TaskType == this._task.TaskType && d.UserID == this._userInfo.UserId).ToList();
                foreach (var d in this.SelectedTaskBucket)
                {
                    this.DocumentList.Add(new Document
                    {
                        CaseNumber = d.CaseNumber,
                        SerialNumber = d.SerialNumber,
                        CaseCategoryRecID = d.CaseCategoryRecID,
                        DocumentType = d.DocumentType,
                        MakeModel = d.MakeModel,
                        RegistrationNumber = d.RegistrationNumber,
                        DocumentName = d.DocumentName,
                        //ActionDate=d.DeliveryDate,
                        //ActionTime = d.DeliveryDate,
                        Status = d.Status,
                        StatusDueDate = d.StatusDueDate
                    });
                }

                switch (this._userInfo.CDUserType)
                {
                    case CDUserType.Courier:
                        this.ContactPersons = await SqliteHelper.Storage.LoadTableAsync<Courier>();
                        break;

                    case CDUserType.Driver:
                        this.ContactPersons = await SqliteHelper.Storage.LoadTableAsync<Driver>();

                        break;
                    case CDUserType.Customer:
                        var contactPersonsData = await SqliteHelper.Storage.LoadTableAsync<CDCustomer>();
                        this.ContactPersons = contactPersonsData;
                        this.SelectedContact = contactPersonsData.Where(s => s.Isprimary).FirstOrDefault();
                        break;
                }

                this.CollectedFrom = await SqliteHelper.Storage.LoadTableAsync<CollectedFromData>();
                this.AlternateContactPersons = (await SqliteHelper.Storage.LoadTableAsync<AlternateContactPerson>()).Where(u => u.UserId ==  this._userInfo.UserId);
                this.IsBusy = false;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                this.IsBusy = false;
            }
        }
        public DelegateCommand CollectCommand { get; set; }
        public DelegateCommand CompleteCommand { get; set; }
        public DelegateCommand AddContactCommand { get; set; }

        public DelegateCommand<ObservableCollection<object>> DocumentsChangedCommand { get; set; }

        private Visibility completeVisibility;

        public Visibility CompleteVisibility
        {
            get { return completeVisibility; }
            set { SetProperty(ref completeVisibility, value); }
        }


        private DocumentCollectDetail documentCollectDetail;

        public DocumentCollectDetail DocumentCollectDetail
        {
            get { return documentCollectDetail; }
            set { SetProperty(ref documentCollectDetail, value); }
        }

        private DocumnetDeliverDetail documnetDeliverDetail;

        public DocumnetDeliverDetail DocumnetDeliverDetail
        {
            get { return documnetDeliverDetail; }
            set { SetProperty(ref documnetDeliverDetail, value); }
        }

        private Visibility collectVisibility;
        public Visibility CollectVisibility
        {
            get { return collectVisibility; }
            set { SetProperty(ref collectVisibility, value); }
        }

        private Visibility isCollSignatureDate;

        public Visibility IsCollSignatureDate
        {
            get { return isCollSignatureDate; }
            set { SetProperty(ref isCollSignatureDate, value); }
        }

        private Visibility isDelSignatureDate;

        public Visibility IsDelSignatureDate
        {
            get { return isDelSignatureDate; }
            set { SetProperty(ref isDelSignatureDate, value); }
        }

        private bool isAlternateOn;
        public bool IsAlternateOn
        {
            get { return isAlternateOn; }
            set
            {
                if (SetProperty(ref isAlternateOn, value))
                {
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool masterIsChecked;
        [Ignore]
        public bool MasterIsChecked
        {
            get { return masterIsChecked; }
            set
            {
                if (SetProperty(ref masterIsChecked, value))
                {
                    foreach (var item in this.DocumentList)
                    {
                        item.IsMarked = masterIsChecked;
                    }
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private string proofTitle;
        public string ProofTitle
        {
            get { return proofTitle; }
            set { SetProperty(ref proofTitle, value); }
        }
        private CollectDeliveryDetail docuDeliveryDetails;
        public CollectDeliveryDetail DocuDeliveryDetails
        {
            get { return docuDeliveryDetails; }
            set { SetProperty(ref docuDeliveryDetails, value); }
        }
        private ObservableCollection<Document> documentList;
        public ObservableCollection<Document> DocumentList
        {
            get { return documentList; }
            set { SetProperty(ref documentList, value); }
        }

        private CDCustomerDetails customerDetails;

        public CDCustomerDetails CustomerDetails
        {
            get { return customerDetails; }
            set { SetProperty(ref customerDetails, value); }
        }

        private string title;
        public string CDTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private DocumentsReceiver selectedContact;
        [Ignore]
        public DocumentsReceiver SelectedContact
        {
            get { return selectedContact; }
            set
            {
                if (SetProperty(ref selectedContact, value))
                {
                    this.ContactNameBorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.SlateBlue);
                    this.DocuDeliveryDetails.DeliveryPersonName = this.SelectedContact.UserName;
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private AlternateContactPerson selectedAlternateContact;
        [Ignore]
        public AlternateContactPerson SelectedAlternateContact
        {
            get { return selectedAlternateContact; }
            set
            {
                if (SetProperty(ref selectedAlternateContact, value))
                {
                    this.ContactNameBorderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.SlateBlue);
                    this.DocuDeliveryDetails.DeliveryPersonName = this.selectedAlternateContact.FullName;
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private CollectedFromData selectedCollectedFrom;
        [Ignore]
        public CollectedFromData SelectedCollectedFrom
        {
            get { return selectedCollectedFrom; }
            set { SetProperty(ref selectedCollectedFrom, value); }
        }


        private IEnumerable<DocumentsReceiver> contactPersons;
        [Ignore]
        public IEnumerable<DocumentsReceiver> ContactPersons
        {
            get { return contactPersons; }
            set
            {
                SetProperty(ref contactPersons, value);
            }
        }

        private IEnumerable<AlternateContactPerson> alternateContactPersons;
        [Ignore]
        public IEnumerable<AlternateContactPerson> AlternateContactPersons
        {
            get { return alternateContactPersons; }
            set
            {
                SetProperty(ref alternateContactPersons, value);
            }
        }

        private BitmapImage cRSignature;
        [Ignore]
        public BitmapImage CRSignature
        {
            get { return cRSignature; }
            set
            {
                if (SetProperty(ref cRSignature, value))
                {
                    if (this.CollectVisibility == Visibility.Visible)
                    {
                        this.IsCollSignatureDate = Visibility.Visible;
                        this.DocuDeliveryDetails.ReceivedDate = DateTime.Now;
                    }

                    if (this.CompleteVisibility == Visibility.Visible)
                    {
                        this.IsDelSignatureDate = Visibility.Visible;
                        this.DocuDeliveryDetails.DeliveryDate = DateTime.Now;
                    }
                    this.CompleteCommand.RaiseCanExecuteChanged();
                    this.CollectCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private List<CollectedFromData> collectedFrom;
        [Ignore]
        public List<CollectedFromData> CollectedFrom
        {
            get { return collectedFrom; }
            set { SetProperty(ref collectedFrom, value); }
        }

        private string ackDialog;
        public string AckDialog
        {
            get { return ackDialog; }
            set { SetProperty(ref ackDialog, value); }
        }

        private List<CollectDeliveryTask> allTaskOfCustomer;

        public List<CollectDeliveryTask> SelectedTaskBucket
        {
            get { return allTaskOfCustomer; }
            set { SetProperty(ref allTaskOfCustomer, value); }
        }
        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool isDeliverType;

        public bool IsDeliverType
        {
            get { return isDeliverType; }
            set { SetProperty(ref isDeliverType, value); }
        }

        private Brush contactNameBorderBrush;
        public Brush ContactNameBorderBrush
        {
            get { return contactNameBorderBrush; }
            set { SetProperty(ref contactNameBorderBrush, value); }
        }

        //private ObservableCollection<Document> selectedDocuments;
        //public ObservableCollection<Document> SelectedDocuments
        //{
        //    get { return selectedDocuments; }
        //    set
        //    {
        //        SetProperty(ref selectedDocuments, value);
        //    }
        //}



        public CDUserInfo _userInfo { get; set; }
    }
}
