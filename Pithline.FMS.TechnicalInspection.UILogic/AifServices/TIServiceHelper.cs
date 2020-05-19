
using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.TI;
using Pithline.FMS.TechnicalInspection.UILogic.TIService;
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
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Pithline.FMS.TechnicalInspection.UILogic.AifServices
{
    public class TIServiceHelper
    {
        private static readonly TIServiceHelper instance = new TIServiceHelper();
        private MzkTechnicalInspectionClient client;
        ConnectionProfile _connectionProfile;
        IEventAggregator _eventAggregator;
        Action _syncExecute;
        private UserInfo _userInfo;

        static TIServiceHelper()
        {

        }

        public static TIServiceHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public MzkTechnicalInspectionClient ConnectAsync(string userName, string password, IEventAggregator eventAggregator, string domain = "lfmd")
        {
            try
            {
                _eventAggregator = eventAggregator;
                BasicHttpBinding basicHttpBinding = new BasicHttpBinding()
                {
                    MaxBufferPoolSize = int.MaxValue,
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue,
                    OpenTimeout = new TimeSpan(2, 0, 0),
                    ReceiveTimeout = new TimeSpan(2, 0, 0),
                    SendTimeout = new TimeSpan(2, 0, 0),
                    AllowCookies = true,

                };
                basicHttpBinding.ReaderQuotas.MaxDepth = int.MaxValue;
                basicHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
                basicHttpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
                basicHttpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
                basicHttpBinding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

                basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://srfmlaxtest01/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc  http://srfmlaxtest01/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc
                client = new MzkTechnicalInspectionClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/TechnicalInspection/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                client.ClientCredentials.UserName.Password = password;
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);
                return client;
            }
            catch (Exception ex)
            {
                return client;
            }
        }

        async public System.Threading.Tasks.Task Synchronize(Action syncExecute)
        {
            _syncExecute = syncExecute;
            _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                await System.Threading.Tasks.Task.Factory.StartNew(syncExecute);
                //_syncExecute.Invoke();
            }
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
            {
                _syncExecute.Invoke();
            }

        }

        async public System.Threading.Tasks.Task Synchronize()
        {
            try
            {

                AppSettings.Instance.IsSynchronizing = 1;


                await Synchronize(async () =>
                    {

                        await InsertTechnicalInspectionAsync();
                        await UpdateTaskStatusAsync();

                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            AppSettings.Instance.IsSynchronizing = 0;
                        });
                    });

            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace;
                });
            }
        }

        async public System.Threading.Tasks.Task SyncImagesAsync()
        {
            try
            {
                var mzk_ImageContractList = new ObservableCollection<Mzk_ImageContract>();
                var taskList = await SqliteHelper.Storage.LoadTableAsync<TITask>();

                foreach (var task in taskList.Where(x => x.Status == Pithline.FMS.BusinessLogic.Helpers.TaskStatus.Completed))
                {
                    var imageTable = await SqliteHelper.Storage.LoadTableAsync<ImageCapture>();
                    var imageCaptureList = imageTable.Where(x => x.CaseServiceRecId == task.CaseServiceRecID);

                    foreach (var item in imageCaptureList.Where(x => !x.IsSynced))
                    {
                        mzk_ImageContractList.Add(new Mzk_ImageContract
                        {
                            parmCaseNumber = task.CaseNumber,
                            parmFileName = string.Format("{0}{1}", item.FileName, ".png"),
                            parmImageData = item.ImageBinary,
                        });

                    }
                    await AddImageMetadataFileAsync(mzk_ImageContractList, task, imageCaptureList);


                    await client.saveImageAsync(mzk_ImageContractList);

                    await SqliteHelper.Storage.DeleteBulkAsync(imageCaptureList);

                    await SqliteHelper.Storage.DeleteSingleRecordAsync(task);

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static async System.Threading.Tasks.Task AddImageMetadataFileAsync(ObservableCollection<Mzk_ImageContract> mzk_ImageContractList, TITask task, IEnumerable<ImageCapture> imageCaptureList)
        {
            var txtFilenName = string.Format("{0}{1}", task.CaseNumber, ".txt");
            var txtFileContent = string.Format("{0}|{1}", task.CaseNumber, String.Join("|", imageCaptureList.Select(x => x.FileName)));
            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(txtFilenName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, txtFileContent);
            var fileStream = await file.OpenAsync(FileAccessMode.Read);

            var contentBytes = new byte[fileStream.Size];
            using (var dataReader = new DataReader(fileStream.GetInputStreamAt(0)))
            {
                await dataReader.LoadAsync((uint)fileStream.Size);
                dataReader.ReadBytes(contentBytes);
            }

            mzk_ImageContractList.Add(new Mzk_ImageContract
            {
                parmCaseNumber = task.CaseNumber,
                parmFileName = txtFilenName,
                parmImageData = Convert.ToBase64String(contentBytes)
            });
        }

        async public System.Threading.Tasks.Task SyncImagesAsync(long caseServiceRecId)
        {
            try
            {
                var mzk_ImageContractList = new ObservableCollection<Mzk_ImageContract>();
                var task = await SqliteHelper.Storage.GetSingleRecordAsync<TITask>(x => x.CaseServiceRecID == caseServiceRecId);
                var imageCaptureList = await SqliteHelper.Storage.LoadTableAsync<ImageCapture>();
                foreach (var item in imageCaptureList.Where(x => x.CaseServiceRecId == caseServiceRecId))
                {
                    mzk_ImageContractList.Add(new Mzk_ImageContract
                        {
                            parmCaseNumber = task.CaseNumber,
                            parmFileName = string.Format("{0}{1}", item.FileName, ".png"),
                            parmImageData = item.ImageBinary,
                        });

                }

                await client.saveImageAsync(mzk_ImageContractList);


                foreach (var item in imageCaptureList.Where(x => x.CaseServiceRecId == caseServiceRecId))
                {
                    item.IsSynced = true;
                    await SqliteHelper.Storage.UpdateSingleRecordAsync(item);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        async public System.Threading.Tasks.Task<MzkTechnicalInspectionValidateUserResponse> ValidateUser(string userId, string password)
        {
            try
            {

                return await client.validateUserAsync(userId, password);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        async public System.Threading.Tasks.Task<MzkTechnicalInspectionGetUserDetailsResponse> GetUserInfoAsync(string userId)
        {
            try
            {
                return await client.getUserDetailsAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        async public System.Threading.Tasks.Task SyncFromSvcAsync(BaseModel baseModel)
        {
            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                if (AppSettings.Instance.IsSynchronizing == 0)
                {
                    Synchronize(async () =>
                    {
                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 1;
                        });

                        if (baseModel is TIData)
                        {
                            await this.InsertTechnicalInspectionAsync();
                        }

                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {

                            AppSettings.Instance.IsSynchronizing = 0;
                        }
                              );
                    });
                }
            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                });
            }
        }


        public async System.Threading.Tasks.Task<List<Pithline.FMS.BusinessLogic.TITask>> SyncTasksFromAXAsync()
        {
            var insertList = new List<Pithline.FMS.BusinessLogic.TITask>();
            try
            {
                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var subCompUpdateList = new List<MaintenanceRepair>();
                var subCompInsertList = new List<MaintenanceRepair>();

                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);
                if (result != null)
                {
                    foreach (var task in result.response)
                    {
                        var tiTask = new Pithline.FMS.BusinessLogic.TITask
                        {
                            CaseServiceRecID = task.parmCaseServiceRecID,
                            ServiceRecID = task.parmCaseRecID,
                            CaseNumber = task.parmCaseId,
                            CaseCategory = task.parmCaseCategory,
                            ContactName = task.parmContactPersonName,
                            ContactNumber = task.parmContactPersonPhone,
                            CustPhone = task.parmCustPhone,
                            CustomerName = task.parmCustName,
                            Status = task.parmStatus,
                            StatusDueDate = task.parmStatusDueDate,
                            UserId = task.parmUserID,
                            Address = task.parmContactPersonAddress,
                            CustomerId = task.parmCustAccount,
                            VehicleInsRecId = task.parmCaseServiceRecID,
                            ConfirmedDate = task.parmInspectionDueDate,
                            AllocatedTo = _userInfo.Name,
                            Email = task.parmEmail

                        };


                        var subComponents = await client.getSubComponentsAsync(new ObservableCollection<long> { tiTask.CaseServiceRecID }, _userInfo.CompanyId);
                        var allSubComponents = await SqliteHelper.Storage.LoadTableAsync<MaintenanceRepair>();

                        if (subComponents != null)
                        {
                            foreach (var item in subComponents.response)
                            {
                                var subComponent = new MaintenanceRepair
                                {
                                    SubComponent = item.parmSubComponent,
                                    MajorComponent = item.parmMajorComponent,
                                    Action = item.parmAction,
                                    Cause = item.parmCause,
                                    CaseServiceRecId = tiTask.CaseServiceRecID,
                                    Repairid = item.parmRecID

                                };

                                if (!allSubComponents.Any(x => x.Repairid == item.parmRecID))
                                {
                                    await SqliteHelper.Storage.InsertSingleRecordAsync(subComponent);

                                }
                            }
                        }

                        insertList.Add(tiTask);

                    }
                    await SqliteHelper.Storage.DropnCreateTableAsync<Pithline.FMS.BusinessLogic.TITask>();

                    await SqliteHelper.Storage.InsertAllAsync(insertList);

                }


            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                  {

                                      AppSettings.Instance.IsSynchronizing = 0;
                                      AppSettings.Instance.ErrorMessage = ex.Message + ex.InnerException;
                                  });
            }
            return insertList;

        }
        public async System.Threading.Tasks.Task InsertTechnicalInspectionAsync()
        {
            try
            {

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var technicalInspData = (await SqliteHelper.Storage.LoadTableAsync<Pithline.FMS.BusinessLogic.TI.TIData>()).Where(x => x.ShouldSave);
                ObservableCollection<MzkCaseServiceAuthorizationContract> mzkMobiTrailerAccessoriesContractColl = new ObservableCollection<MzkCaseServiceAuthorizationContract>();
                if (technicalInspData != null)
                {
                    foreach (var x in technicalInspData)
                    {
                        mzkMobiTrailerAccessoriesContractColl.Add(new MzkCaseServiceAuthorizationContract()
                            {
                                parmDamageCause = x.CauseOfDamage,
                                parmRemedy = x.Remedy,
                                parmRecommendation = x.Recommendation,
                                parmCompletionDate = x.CompletionDate,
                                // parmCaseCategoryAuthList = x.CaseCategoryAuthList,
                                parmCaseServiceRecID = x.VehicleInsRecID

                            });
                    }
                }
                var res = await client.insertTechnicalInspectionAsync(mzkMobiTrailerAccessoriesContractColl, _userInfo.CompanyId);

                var technicalInspList = new ObservableCollection<TIData>();
                if (res.response != null)
                {
                    foreach (var x in res.response.Where(x => x != null))
                    {

                        technicalInspList.Add(new TIData
                        {
                            CauseOfDamage = x.parmDamageCause,
                            Remedy = x.parmRemedy,
                            Recommendation = x.parmRecommendation,
                            CompletionDate = x.parmCompletionDate,
                            ShouldSave = false,
                            // parmCaseCategoryAuthList = x.CaseCategoryAuthList,
                            VehicleInsRecID = x.parmCaseServiceRecID

                        });
                    }




                    await SqliteHelper.Storage.DeleteBulkAsync<TIData>(technicalInspList);

                    foreach (var item in res.response.Where(x => x != null))
                    {
                        await SyncImagesAsync(item.parmCaseServiceRecID);
                    }
                }
            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                });
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
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var tasks = (await SqliteHelper.Storage.LoadTableAsync<Pithline.FMS.BusinessLogic.TITask>());
                ObservableCollection<MzkTechnicalTasksContract> mzkTasks = new ObservableCollection<MzkTechnicalTasksContract>();
                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();
                actionStepMapping.Add(Pithline.FMS.BusinessLogic.Helpers.TaskStatus.Completed, EEPActionStep.AwaitTechnicalInspection);

                if (tasks != null)
                {

                    foreach (var task in tasks.Where(x =>
                        x.Status == Pithline.FMS.BusinessLogic.Helpers.TaskStatus.Completed))
                    {
                        mzkTasks.Add(
                            new MzkTechnicalTasksContract

                            {
                                parmCaseServiceRecID = task.CaseServiceRecID,
                                parmServiceRecID = task.ServiceRecID,
                                parmCaseId = task.CaseNumber,
                                parmCustAddress = task.Address,
                                parmCustName = task.CustomerName,
                                parmCustPhone = task.CustPhone,
                                parmContactPersonPhone = task.ContactNumber,
                                parmContactPersonName = task.ContactName,
                                parmStatus = task.Status,
                                parmStatusDueDate = task.StatusDueDate,
                                parmUserID = task.UserId,
                                parmEEPActionStep = actionStepMapping[task.Status]
                            });
                    }
                    var res = await client.updateStatusListAsync(mzkTasks, _userInfo.CompanyId);


                    if (res.response != null)
                    {
                        foreach (var item in tasks)
                        {
                            if (res.response.Any(x => x.parmCaseId == item.CaseNumber))
                            {
                                item.Status = res.response.Single(x => x.parmCaseId == item.CaseNumber).parmStatus;
                                await SqliteHelper.Storage.UpdateSingleRecordAsync(item);
                            }
                        }
                    }

                }
            }
            catch (SQLite.SQLiteException)
            {

            }
            catch (Exception ex)
            {
                Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {

                    AppSettings.Instance.IsSynchronizing = 0;
                    AppSettings.Instance.ErrorMessage = ex.Message;
                });
            }

        }
    }
}
