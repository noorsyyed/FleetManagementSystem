using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.DocumentDelivery;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.DDServiceProxy;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;

namespace Pithline.FMS.DocumentDelivery.UILogic.AifServices
{
    public class DDServiceProxyHelper
    {
        private static readonly DDServiceProxyHelper instance = new DDServiceProxyHelper();
        private MzkCollectDeliveryServiceClient _client;
        IEventAggregator _eventAggregator;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private CDUserInfo _userInfo;
        public static DDServiceProxyHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public async System.Threading.Tasks.Task<MzkCollectDeliveryServiceClient> ConnectAsync(string userName, string password, IEventAggregator eventAggregator, string domain = "lfmd")
        {
            try
            {

                try
                {
                    if (this._userInfo == null)
                    {
                        this._userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                    }

                }
                catch (Exception)
                {

                }
                _eventAggregator = eventAggregator;
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding()
                {
                    MaxBufferPoolSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    OpenTimeout = new TimeSpan(2, 0, 0),
                    ReceiveTimeout = new TimeSpan(2, 0, 0),
                    SendTimeout = new TimeSpan(2, 0, 0),
                    AllowCookies = true
                };

                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://SRFMLAXTEST01/MicrosoftDynamicsAXAif60/CollectDeliveryService/xppservice.svc
                _client = new MzkCollectDeliveryServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/CollectDeliverService/xppservice.svc"));
                _client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                _client.ClientCredentials.UserName.Password = password;
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return _client;
            }
            catch (Exception ex)
            {
                Util.ShowToast(ex.Message);
                return _client;
            }
        }
        public async System.Threading.Tasks.Task SendMessageToUIThread(string receivedMsg)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {

                AppSettings.Instance.IsSynchronizing = 0;
                AppSettings.Instance.ErrorMessage = receivedMsg;
            });
        }
        async public System.Threading.Tasks.Task<CDUserInfo> ValidateUser(string userId, string password)
        {
            try
            {
                var result = await _client.validateUserAsync(userId, password);

                if (result != null && result.response != null)
                {
                    var userInfo = new CDUserInfo
                    {
                        UserId = result.response.parmUserID,
                        CompanyId = result.response.parmCompany,
                        CompanyName = result.response.parmCompanyName,
                        Name = result.response.parmUserName,
                        CDUserType = (CDUserType)Enum.Parse(typeof(CDUserType), result.response.parmLoginType.ToString()),
                    };

                    this._userInfo = userInfo;
                    return userInfo;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Synchronize(Action syncExecute)
        {
            _syncExecute = syncExecute;
            _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                System.Threading.Tasks.Task.Factory.StartNew(syncExecute);

            }
        }
        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            try
            {
                if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    _syncExecute.Invoke();
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);

            }

        }
        async public System.Threading.Tasks.Task SynchronizeAllAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    this._userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                Synchronize(async () =>
                   {
                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                       {
                           AppSettings.Instance.IsSynchronizing = 1;
                       });


                       await this.InsertDocumentDetailsCollectedByCustomerToSvcAsync();
                       await this.GetDriverListFromSvcAsync();
                       await this.GetCourierListFromSvcAsync();
                       //await      this.InsertDocumentCollectedByDriverOrCourierToSvcAsync();
                       await this.UpdateDeliveryDetailAsync();
                       await this.InsertDocumentDeliveredByDriverOrCourierToSvcAsync();
                       await this.UpdateTaskStatusAsync();
                       await this.GetCollectedFromSvcAsync();

                       await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                       {
                           AppSettings.Instance.IsSynchronizing = 0;
                       });

                   });

            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }

        async private System.Threading.Tasks.Task UpdateDeliveryDetailAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (this._userInfo == null)
                {
                    this._userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumentDeliveryUpdateDetail>());
                ObservableCollection<MZKCourierCollectionDetailsContract> mzkDocumentCollectedDetailsContractColl = new ObservableCollection<MZKCourierCollectionDetailsContract>();

                foreach (var doc in cdDataList)
                {
                    mzkDocumentCollectedDetailsContractColl.Add(new MZKCourierCollectionDetailsContract()
                    {
                        parmCaseId = doc.CaseNumber,
                        parmCollectedFrom = doc.SelectedCollectedFrom,
                        parmComment = doc.Comment,
                        parmEmail = doc.Email,
                        parmCaseServiceRecId = doc.CaseServiceRecId,
                        parmContactPerson = doc.DeliveryPersonName,
                        parmCollectedBy = doc.DeliveryPersonName,
                        parmPosition = doc.Position,
                        parmTelePhone = doc.Phone,
                        // parmContactNumber
                        // parmCourierSignature = doc.CRSignature.,
                        parmReceivedBy = doc.DeliveryPersonName,
                        parmDateTime = doc.DeliveryDate,
                        parmContactNumber = doc.Phone
                    });

                }
                if (mzkDocumentCollectedDetailsContractColl.Count > 0)
                {
                    var res = await _client.updateDeliveryDetailsAsync(mzkDocumentCollectedDetailsContractColl, _userInfo.CompanyId);
                    if (res != null && res.response.Any())
                    {
                        foreach (var cdData in cdDataList)
                        {
                            if (res.response.Any(a => a.parmCaseId == cdData.CaseNumber && a.parmCaseServiceRecId == cdData.CaseServiceRecId))
                            {
                                await SqliteHelper.Storage.DeleteSingleRecordAsync<DocumentDeliveryUpdateDetail>(cdData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        public async System.Threading.Tasks.Task<ObservableCollection<CollectDeliveryTask>> GroupTasksByCustomer()
        {
            try
            {
                var allTaskList = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>())
                    .Where(
                    w => w.Status != CDTaskStatus.Completed && w.AllocatedTo == _userInfo.UserId);
                var taskbuckets = allTaskList.GroupBy(x => new { x.CustomerId, x.ContactPersonAddress, x.TaskType })
                    .Select(
                    s => new
                    {
                        taskdata = s.First(),
                        count = s.Count()
                    });

                ObservableCollection<CollectDeliveryTask> tasks = new ObservableCollection<CollectDeliveryTask>();
                foreach (var singleTask in taskbuckets)
                {
                    if (singleTask != null)
                    {
                        //singleTask.CDTasksDetails = new ObservableCollection<CDTaskDetails>();
                        //foreach (var item in allTaskList)
                        //{
                        //    if (item != null && item.CustomerId == singleTask.CustomerId && item.TaskType == singleTask.TaskType)
                        //    {
                        //        singleTask.CDTasksDetails.Add(new CDTaskDetails
                        //        {
                        //            CaseNumber = item.CaseNumber,
                        //            CustomerName = item.CustomerName,
                        //            TaskType = item.TaskType,
                        //            ContactName = item.ContactName,
                        //            Address = item.Address.Replace(System.Environment.NewLine, ","),
                        //            DocumentCount = item.DocumentCount,
                        //            AllocatedTo = item.AllocatedTo,
                        //            CaseCategoryRecID = item.CaseCategoryRecID,
                        //            SerialNumber = item.SerialNumber,
                        //            RegistrationNumber = item.RegistrationNumber,
                        //            MakeModel = item.MakeModel,
                        //            Status = item.Status,
                        //            StatusDueDate = item.StatusDueDate,
                        //            DeliveryDate = item.DeliveryDate,

                        //        });

                        //    }
                        //}
                        if (singleTask.taskdata.TaskType != CDTaskType.None)
                        {
                            singleTask.taskdata.DocumentCount = singleTask.count;
                            tasks.Add(singleTask.taskdata);
                        }
                    }
                }
                return tasks;
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
                throw;
            }
        }
        async public System.Threading.Tasks.Task<List<CollectDeliveryTask>> SyncTasksFromSvcAsync()
        {
            List<CollectDeliveryTask> taskInsertList = new List<CollectDeliveryTask>();

            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var result = await _client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
                var taskData = await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>();
                ObservableCollection<long> caseCategoryRecIdList = new ObservableCollection<long>();
                if (result.response != null)
                {
                    foreach (var mzkTask in result.response.GroupBy(g => g.parmCaseId).Select(s => s.First()))
                    {
                        var taskTosave = new CollectDeliveryTask
                          {
                              CaseNumber = mzkTask.parmCaseId,
                              Address = mzkTask.parmContactPersonAddress,
                              CustomerName = mzkTask.parmCustName,
                              CustomerNumber = mzkTask.parmContactPersonPhone,
                              Status = mzkTask.parmStatus,
                              StatusDueDate = mzkTask.parmStatusDueDate,
                              RegistrationNumber = mzkTask.parmRegNo,
                              AllocatedTo = mzkTask.parmUserID,
                              MakeModel = mzkTask.parmMake + Environment.NewLine + mzkTask.parmModel,
                              CaseCategoryRecID = mzkTask.parmCaseCategoryRecID,
                              DeliveryDate = mzkTask.parmDeliveryDateTime,
                              EmailId = mzkTask.parmCustomerEmail,
                              CustPartyId = mzkTask.parmCustPartyId,
                              CaseRecID = mzkTask.parmCaseRecID,
                              CaseServiceRecID = mzkTask.parmCaseServiceRecID,
                              TaskType = (CDTaskType)Enum.Parse(typeof(CDTaskType), mzkTask.parmCollectDeliverType.ToString()),
                              CustomerId = mzkTask.parmCustAccount,
                              ServiceId = mzkTask.parmServiceId,
                              ServiceRecID = mzkTask.parmServiceRecID,
                              UserID = _userInfo.UserId,
                              SerialNumber = mzkTask.parmSerialNumber,
                              ContactName = mzkTask.parmContactPersonName,
                              ContactPersonAddress = mzkTask.parmContactPersonAddress,
                              DocumentType = mzkTask.parmDocuTypeID,
                              DocumentName = mzkTask.parmDocuName,

                          };

                        taskInsertList.Add(taskTosave);

                        caseCategoryRecIdList.Add(mzkTask.parmCaseCategoryRecID);
                        await this.GetCustomerListFromSvcAsync(mzkTask.parmCustAccount);
                    }
                    await SqliteHelper.Storage.DropnCreateTableAsync<CollectDeliveryTask>();

                    await SqliteHelper.Storage.InsertAllAsync(taskInsertList);

                }
                await GetCollectedFromSvcAsync();
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);

            }
            return taskInsertList;
        }
        async public System.Threading.Tasks.Task GetCollectedFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var collectedFromData = (await SqliteHelper.Storage.LoadTableAsync<CollectedFromData>());
                var result = await _client.getCollectedFromAsync(_userInfo.CompanyId);
                if (result.response != null)
                {
                    foreach (var mzk in result.response)
                    {
                        if (collectedFromData != null && collectedFromData.Any(a => a.UserID == mzk.parmUserID))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<CollectedFromData>(new CollectedFromData { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }
                        else
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<CollectedFromData>(new CollectedFromData { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        async public System.Threading.Tasks.Task GetCustomerListFromSvcAsync(string customerId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess || _userInfo.CDUserType != CDUserType.Customer)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var customerData = (await SqliteHelper.Storage.LoadTableAsync<CDCustomer>());
                var result = await _client.getCustomerInfoAsync(customerId, _userInfo.CompanyId);
                if (result.response != null)
                {
                    foreach (var mzk in result.response)
                    {
                        if (customerData != null && customerData.Any(a => a.UserID == mzk.parmContactPersonId))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<CDCustomer>(new CDCustomer { UserID = mzk.parmContactPersonId, UserName = mzk.parmContactPersonName, Address = mzk.parmCustAddress });
                        }
                        else
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<CDCustomer>(new CDCustomer { UserID = mzk.parmContactPersonId, UserName = mzk.parmContactPersonName, Address = mzk.parmCustAddress });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        async public System.Threading.Tasks.Task GetCourierListFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess || _userInfo.CDUserType != CDUserType.Courier)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var courierData = (await SqliteHelper.Storage.LoadTableAsync<Courier>());
                var result = await _client.getCourierListAsync(_userInfo.CompanyId);
                if (result.response != null)
                {
                    foreach (var mzk in result.response)
                    {
                        if (courierData != null && courierData.Any(a => a.UserID == mzk.parmUserID))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<Courier>(new Courier { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }
                        else
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<Courier>(new Courier { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        async public System.Threading.Tasks.Task GetDriverListFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess || _userInfo.CDUserType != CDUserType.Driver)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var courierData = (await SqliteHelper.Storage.LoadTableAsync<Driver>());
                var result = await _client.getDriversAsync(_userInfo.CompanyId);
                if (result.response != null)
                {
                    foreach (var mzk in result.response)
                    {
                        if (courierData != null && courierData.Any(a => a.UserID == mzk.parmUserID))
                        {
                            await SqliteHelper.Storage.UpdateSingleRecordAsync<Driver>(new Driver { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }
                        else
                        {
                            await SqliteHelper.Storage.InsertSingleRecordAsync<Driver>(new Driver { UserID = mzk.parmUserID, UserName = mzk.parmUserName, Address = mzk.parmAddress });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }

        //async public System.Threading.Tasks.Task GetCollectedFromAddressSvcAsync()
        //{
        //    try
        //    {
        //        var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
        //        if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
        //            return;

        //        if (_userInfo == null)
        //        {
        //            _userInfo = PersistentData.Instance.UserInfo;
        //        }
        //        var collectedFromData = (await SqliteHelper.Storage.LoadTableAsync<CollectedFromData>());
        //        var result = await _client.getCollectedFromAddressAsync(_userInfo.UserId, _userInfo.CompanyId);
        //        if (result.response != null)
        //        {
        //            foreach (var mzk in result.response)
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.SendMessageToUIThread(ex.Message);
        //    }
        //}
        async public System.Threading.Tasks.Task InsertDocumentDetailsCollectedByCustomerToSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                //if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess || _userInfo.CDUserType != CDUserType.Customer)

                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumentCollectDetail>());
                ObservableCollection<MZKDocumentCollectedDetailsContract> mzkDocumentCollectedDetailsContractColl = new ObservableCollection<MZKDocumentCollectedDetailsContract>();

                foreach (var doc in cdDataList)
                {
                    mzkDocumentCollectedDetailsContractColl.Add(new MZKDocumentCollectedDetailsContract()
                    {
                        parmCaseId = doc.CaseNumber,
                        parmCollectedFrom = doc.SelectedCollectedFrom,
                        parmComment = doc.Comment,
                        parmEmail = doc.Email,
                        parmPosition = doc.Position,
                        parmReceivedBy = doc.ReceivedBy,
                        parmReceivedDate = doc.ReceivedDate,
                        parmReferenceNo = doc.ReferenceNo,
                        parmCaseServiceRecId = doc.CaseServiceRecId,
                        // parmSignature = doc.CRSignature,
                        parmTelePhone = doc.Phone

                    });
                }

                if (mzkDocumentCollectedDetailsContractColl.Count > 0)
                {
                    var res = await _client.insertDocumentCustomerDetailsAsync(mzkDocumentCollectedDetailsContractColl, _userInfo.CompanyId);

                    if (res != null && res.response.Any())
                    {

                        foreach (var cdData in cdDataList)
                        {
                            if (res.response.Any(a => a.parmCaseId == cdData.CaseNumber && a.parmCaseServiceRecId == cdData.CaseServiceRecId))
                            {
                                await SqliteHelper.Storage.DeleteSingleRecordAsync<DocumentCollectDetail>(cdData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        //async public System.Threading.Tasks.Task InsertDocumentCollectedByDriverOrCourierToSvcAsync()
        //{
        //    try
        //    {
        //        var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
        //        if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess || _userInfo.CDUserType == CDUserType.Customer)
        //            return;

        //        if (_userInfo == null)
        //        {
        //            _userInfo = PersistentData.Instance.UserInfo;
        //        }
        //        var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumentCollectDetail>());
        //        ObservableCollection<MZKCourierCollectionDetailsContract> mzkDocumentCollectedDetailsContractColl = new ObservableCollection<MZKCourierCollectionDetailsContract>();

        //            foreach (var doc in cdDataList)
        //            {
        //                mzkDocumentCollectedDetailsContractColl.Add(new MZKCourierCollectionDetailsContract()
        //                {
        //                    parmCaseId = doc.CaseNumber,
        //                    parmCollectedFrom = doc.SelectedCollectedFrom,
        //                    parmComment = doc.Comment,
        //                    parmEmail = doc.Email,
        //                    parmCaseServiceRecId = doc.CaseServiceRecId,
        //                    parmContactPerson = doc.ReceivedBy,
        //                    parmCollectedBy = doc.ReceivedBy,
        //                    parmPosition = doc.Position,
        //                    parmTelePhone = doc.Phone,
        //                    // parmContactNumber
        //                    // parmCourierSignature = doc.CRSignature.,
        //                    parmReceivedBy = doc.ReceivedBy,
        //                    parmDateTime = doc.ReceivedDate,
        //                    parmContactNumber = doc.Phone

        //                });
        //            }
        //            if (mzkDocumentCollectedDetailsContractColl.Count>0)
        //            {

        //                var res = await _client.insertDocumentDriverCourierDetailsAsync(mzkDocumentCollectedDetailsContractColl, _userInfo.CompanyId);
        //                if (res != null && res.response.Any())
        //                {
        //                    foreach (var cdData in cdDataList)
        //                    {
        //                        if (res.response.Any(a => a.parmCaseId == cdData.CaseNumber && a.parmCaseServiceRecId == cdData.CaseServiceRecId))
        //                        {
        //                            await SqliteHelper.Storage.DeleteSingleRecordAsync<DocumentCollectDetail>(cdData);
        //                        }
        //                    }
        //                } 
        //            }

        //    }
        //    catch (Exception ex)
        //    {
        //        this.SendMessageToUIThread(ex.Message);
        //    }
        //}
        async public System.Threading.Tasks.Task InsertDocumentDeliveredByDriverOrCourierToSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var cdDataList = (await SqliteHelper.Storage.LoadTableAsync<DocumnetDeliverDetail>());
                ObservableCollection<MZKCourierCollectionDetailsContract> mzkDocumentCollectedDetailsContractColl = new ObservableCollection<MZKCourierCollectionDetailsContract>();

                foreach (var doc in cdDataList)
                {
                    mzkDocumentCollectedDetailsContractColl.Add(new MZKCourierCollectionDetailsContract()
                    {
                        parmCaseId = doc.CaseNumber,
                        parmCollectedFrom = doc.SelectedCollectedFrom,
                        parmComment = doc.Comment,
                        parmEmail = doc.Email,
                        parmCaseServiceRecId = doc.CaseServiceRecId,
                        parmContactPerson = doc.DeliveryPersonName,
                        parmCollectedBy = doc.DeliveryPersonName,
                        parmPosition = doc.Position,
                        parmTelePhone = doc.Phone,
                        // parmContactNumber
                        // parmCourierSignature = doc.CRSignature.,
                        parmReceivedBy = doc.DeliveryPersonName,
                        parmDateTime = doc.DeliveryDate,
                        parmContactNumber = doc.Phone
                    });

                }
                if (mzkDocumentCollectedDetailsContractColl.Count > 0)
                {
                    var res = await _client.insertDocumentDriverCourierDetailsAsync(mzkDocumentCollectedDetailsContractColl, _userInfo.CompanyId);
                    if (res != null && res.response.Any())
                    {
                        foreach (var cdData in cdDataList)
                        {
                            if (res.response.Any(a => a.parmCaseId == cdData.CaseNumber && a.parmCaseServiceRecId == cdData.CaseServiceRecId))
                            {
                                await SqliteHelper.Storage.DeleteSingleRecordAsync<DocumnetDeliverDetail>(cdData);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }
        }
        public async System.Threading.Tasks.Task UpdateTaskStatusAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return;
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<CDUserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var tasks = (await SqliteHelper.Storage.LoadTableAsync<CollectDeliveryTask>()).Where(w => w.UserID == _userInfo.UserId && w.IsNotSyncedWithAX);

                ObservableCollection<MZKCollectDeliverTasksContract> mzkTasks = new ObservableCollection<MZKCollectDeliverTasksContract>();
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                //actionStepMapping.Add(CDTaskStatus.AwaitLicenceDisc, EEPActionStep.AwaitLicenceDisc);


                switch (_userInfo.CDUserType)
                {
                    case CDUserType.Driver:

                        actionStepMapping.Add(CDTaskStatus.AwaitDeliveryConfirmation, EEPActionStep.AwaitDriverCollection);
                        actionStepMapping[CDTaskStatus.AwaitInvoice] = EEPActionStep.AwaitDriverCollectionLicenseDisc;
                        actionStepMapping.Add(CDTaskStatus.Completed, EEPActionStep.AwaitDeliveryConfirmation);
                        tasks = tasks.Where(w => w.Status == CDTaskStatus.AwaitDeliveryConfirmation || w.Status == CDTaskStatus.Completed ||
                            w.Status == CDTaskStatus.AwaitInvoice);
                        break;

                    case CDUserType.Courier:

                        actionStepMapping.Add(CDTaskStatus.AwaitDeliveryConfirmation, EEPActionStep.AwaitCourierCollection);
                        actionStepMapping.Add(CDTaskStatus.AwaitInvoice, EEPActionStep.AwaitDeliveryConfirmationCourier);
                        tasks = tasks.Where(w => w.Status == CDTaskStatus.AwaitDeliveryConfirmation || w.Status == CDTaskStatus.AwaitInvoice);
                        break;

                    case CDUserType.Customer:
                        actionStepMapping.Add(CDTaskStatus.Completed, EEPActionStep.AwaitCollectionDetail);
                        tasks = tasks.Where(w => w.Status == CDTaskStatus.Completed);
                        break;

                }

                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        //if (task.ServiceId == "Licence Dup")
                        //{
                        //    actionStepMapping[CDTaskStatus.AwaitInvoice] = EEPActionStep.AwaitDriverCollectionLicenseDisc;
                        //}
                        mzkTasks.Add(
                            new MZKCollectDeliverTasksContract
                            {
                                parmCaseId = task.CaseNumber,
                                parmCaseServiceRecID = task.CaseServiceRecID,
                                parmServiceRecID = task.ServiceRecID,
                                parmStatusDueDate = task.StatusDueDate,
                                parmEEPActionStep = actionStepMapping[task.Status],
                                parmServiceId = task.ServiceId
                            });
                    }
                    if (mzkTasks.Count > 0)
                    {
                        var res = await _client.updateStatusListAsync(mzkTasks, (MzkLoginType)Enum.Parse(typeof(MzkLoginType), _userInfo.CDUserType.ToString()), _userInfo.CompanyId);

                        if (res.response != null && res.response.Any())
                        {
                            foreach (var task in tasks)
                            {
                                if (res.response.Any(a => a.parmCaseId == task.CaseNumber))
                                {
                                    if (task.TaskType == CDTaskType.None)
                                    {
                                        await SqliteHelper.Storage.DeleteSingleRecordAsync<CollectDeliveryTask>(task);
                                    }
                                    else
                                    {
                                        task.IsNotSyncedWithAX = false;
                                        await SqliteHelper.Storage.UpdateSingleRecordAsync<CollectDeliveryTask>(task);
                                    }
                                }
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                this.SendMessageToUIThread(ex.Message);
            }

        }
    }
}
