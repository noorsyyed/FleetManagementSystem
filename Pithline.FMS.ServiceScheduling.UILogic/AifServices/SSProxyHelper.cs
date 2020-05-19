using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Pithline.FMS.ServiceScheduling.UILogic.SSProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Xaml.Shapes;
namespace Pithline.FMS.ServiceScheduling.UILogic.AifServices
{
    public class SSProxyHelper
    {
        private static readonly SSProxyHelper instance = new SSProxyHelper();
        private MzkServiceSchedulingServiceClient client;
        ConnectionProfile _connectionProfile;
        Action _syncExecute;
        private UserInfo _userInfo;
        static SSProxyHelper()
        {

        }
        public static SSProxyHelper Instance
        {
            get
            {
                return instance;
            }
        }
        public MzkServiceSchedulingServiceClient ConnectAsync(string userName, string password, string domain = "lfmd")
        {
            try
            {
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://srfmlaxtest01/MicrosoftDynamicsAXAif60/SSService/xppservice.svchttp://SRFMLAXTEST01/MicrosoftDynamicsAXAif60/SSService/xppservice.svc
                client = new MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress(" http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/SSService/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = domain + "\"" + userName;
                client.ClientCredentials.UserName.Password = password;
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential(userName, password, domain);


                return client;
            }
            catch (Exception ex)
            {
                Util.ShowToast(ex.Message);
                return client;
            }
        }

        async public System.Threading.Tasks.Task<MzkServiceSchedulingServiceGetUserDetailsResponse> GetUserInfo(string userId)
        {
            return await client.getUserDetailsAsync(userId);
        }

        async public System.Threading.Tasks.Task<bool> ValidateUser(string userId, string password)
        {
            try
            {

                return !(await client.validateUserAsync(userId, password)).response;
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
                //_syncExecute.Invoke();
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
                AppSettings.Instance.ErrorMessage = ex.Message;

            }

        }
        async public System.Threading.Tasks.Task<List<DriverTask>> GetTasksFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var result = await client.getTasksAsync(_userInfo.UserId, _userInfo.CompanyId);

                List<DriverTask> driverTaskList = new List<DriverTask>();
                if (result.response != null)
                {

                    foreach (var mzkTask in result.response.Where(x => x != null))
                    {
                        driverTaskList.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.DriverTask
                        {
                            CaseNumber = mzkTask.parmCaseID,
                            Address = mzkTask.parmContactPersonAddress,
                            CustomerName = mzkTask.parmCustName,
                            CustPhone = string.IsNullOrEmpty(mzkTask.parmCustPhone) ? "" : "+" + mzkTask.parmCustPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate,
                            RegistrationNumber = mzkTask.parmRegistrationNum,
                            AllocatedTo = _userInfo.Name,
                            UserId = mzkTask.parmUserID,
                            CaseCategory = mzkTask.parmCaseCategory,
                            CaseServiceRecID = mzkTask.parmCaseServiceRecID,
                            DriverFirstName = mzkTask.parmDriverFirstName,
                            DriverLastName = mzkTask.parmDriverLastName,
                            DriverPhone = mzkTask.parmDriverPhone,
                            Make = mzkTask.parmMake,
                            Model = mzkTask.parmModel,
                            Description = mzkTask.parmVehicleDescription,
                            CusEmailId = mzkTask.parmEmail,
                            ServiceRecID = mzkTask.parmServiceRecID,
                            ConfirmationDate = mzkTask.parmConfirmationDate == new DateTime(1900, 1, 1) ? string.Empty : mzkTask.parmConfirmationDate.ToString(),
                            CustomerId = mzkTask.parmCustAccount,
                            ContactName = mzkTask.parmContactPersonName,
                            VehicleClassId = mzkTask.parmVehicleClassId,
                            VehicleSubClassId = mzkTask.parmVehicleSubClassId
                            // ScheduledTime=DateTime.Now
                        });

                    }
                }
                return driverTaskList.OrderByDescending(x => x.CaseNumber).ToList();
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<Country>> GetCountryRegionListFromSvcAsync()
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getCountryRegionListAsync(_userInfo.CompanyId);

                List<Country> countryList = new List<Country>();
                if (result.response != null)
                {

                    foreach (var mzk in result.response.OrderBy(o => o.parmCountryRegionName).Where(x => x != null))
                    {

                        countryList.Add(
                                           new Country
                                           {
                                               Name = mzk.parmCountryRegionName,
                                               Id = mzk.parmCountryRegionId
                                           }
                                           );

                    }
                }

                return countryList;

            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<Province>> GetProvinceListFromSvcAsync(string countryId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getProvinceListAsync(countryId, _userInfo.CompanyId);
                List<Province> provinceList = new List<Province>();
                if (result.response != null)
                {

                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        provinceList.Add(new Province { Name = mzk.parmStateName, Id = mzk.parmStateId });
                    }
                }
                return provinceList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }



        async public System.Threading.Tasks.Task<List<City>> GetCityListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getCityListAsync(countryId, stateId, _userInfo.CompanyId);
                List<City> cityList = new List<City>();
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        cityList.Add(new City { Name = mzk.parmCountyId, Id = mzk.parmCountyId });
                    }
                }
                return cityList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<Suburb>> GetSuburbListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getSuburbListAsync(countryId, stateId, _userInfo.CompanyId);
                List<Suburb> suburbList = new List<Suburb>();
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        suburbList.Add(new Suburb { Name = mzk.parmCity, Id = mzk.parmStateId });
                    }
                }
                return suburbList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<Region>> GetRegionListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getRegionsAsync(countryId, stateId, _userInfo.CompanyId);
                List<Region> regionList = new List<Region>();
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        regionList.Add(new Region { Name = mzk.parmRegionName, Id = mzk.parmRegion });
                    }
                }
                return regionList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<string>> GetZipcodeListFromSvcAsync(string countryId, string stateId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                var result = await client.getZipcodeListAsync(countryId, stateId, _userInfo.CompanyId);
                List<string> zipcodeList = new List<string>();
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        zipcodeList.Add(mzk.parmZipCode);
                    }
                }
                return zipcodeList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<ServiceSchedulingDetail> GetServiceDetailsFromSvcAsync(string caseNumber, long caseServiceRecId, long serviceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getServiceDetailsAsync(caseNumber, caseServiceRecId, _userInfo.CompanyId);
                ServiceSchedulingDetail detailServiceScheduling = null;
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        detailServiceScheduling = (new Pithline.FMS.BusinessLogic.ServiceSchedule.ServiceSchedulingDetail
                        {
                            Address = mzk.parmAddress,
                            AdditionalWork = mzk.parmAdditionalWork,
                            ServiceDateOption1 = mzk.parmPreferredDateFirstOption.Year == 1900 ? DateTime.Today : mzk.parmPreferredDateFirstOption,
                            ServiceDateOption2 = mzk.parmPreferredDateSecondOption.Year == 1900 ? DateTime.Today : mzk.parmPreferredDateSecondOption,
                            ODOReading = mzk.parmODOReading.ToString(),
                            ODOReadingDate = mzk.parmODOReadingDate.Year == 1900 ? DateTime.Today : mzk.parmODOReadingDate,
                            ServiceType = GetServiceTypesAsync(caseNumber, _userInfo.CompanyId),
                            LocationTypes = GetLocationTypeAsync(serviceRecId, _userInfo.CompanyId),
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = string.IsNullOrEmpty(mzk.parmContactPersonPhone) ? "" : "+" + mzk.parmContactPersonPhone,
                            SupplierDateTime = DateTime.Now,// need to add in service
                            SelectedLocRecId = mzk.parmLiftLocationRecId,
                            ODOReadingSnapshot = await GetOdoReadingImageAsync(mzk.parmODOReadingImage),
                            SelectedServiceType = mzk.parmServiceType,
                            IsLiftRequired = mzk.parmLiftRequired == NoYes.Yes ? true : false,
                            ConfirmedDate = mzk.parmConfirmedDate.Year == 1900 ? "" : mzk.parmConfirmedDate.ToString(CultureInfo.CurrentCulture)
                        });
                        detailServiceScheduling.SelectedLocationType = detailServiceScheduling.LocationTypes.Find(x => x.RecID == mzk.parmLocationType.parmRecID);
                    }


                }
                return detailServiceScheduling;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }

        async private Task<ImageCapture> GetOdoReadingImageAsync(string odoReadingImageBinary)
        {
            if (string.IsNullOrEmpty(odoReadingImageBinary))
            {
                return new ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png" };
            }
            var sf = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Odo_" + new Random().Next().ToString() + ".png", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(sf, Convert.FromBase64String(odoReadingImageBinary));

            return new ImageCapture { ImagePath = sf.Path, ImageBinary = odoReadingImageBinary };

        }
        private List<string> GetServiceTypesAsync(string caseNumber, string companyId)
        {
            var result = (client.getServiceTypesAsync(caseNumber, _userInfo.CompanyId).Result).response;
            List<string> results = new List<string>();
            if (result.IndexOf("~") > 1)
            {
                results.AddRange(result.Split('~'));
            }
            else
            {
                results.Add(result);
            }

            return results;
        }
        public List<LocationType> GetLocationTypeAsync(long serviceRecId, string companyId)
        {
            List<LocationType> locationTypes = new List<LocationType>();
            try
            {
                var result = (client.getLocationTypeAsync(serviceRecId, companyId).Result);
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        locationTypes.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.LocationType
                        {
                            LocationName = mzk.parmLocationName,
                            LocType = mzk.parmLocationType.ToString(),
                            RecID = mzk.parmRecID,
                        });
                    }
                }
                return locationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return locationTypes;
            }
        }
        async public System.Threading.Tasks.Task<List<DestinationType>> GetCustomersFromSvcAsync(string cusId)
        {
            List<DestinationType> destinationTypes = new List<DestinationType>();
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getCustomersAsync(cusId, _userInfo.CompanyId);

                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        destinationTypes.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.DestinationType
                       {
                           ContactName = mzk.parmName,
                           Id = mzk.parmAccountNum,
                           RecID = mzk.parmRecID,
                           Address = mzk.parmAddress
                       });
                    }
                }
                destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return destinationTypes;
            }
        }
        async public System.Threading.Tasks.Task<List<DestinationType>> GetVendorsFromSvcAsync()
        {
            List<DestinationType> destinationTypes = new List<DestinationType>();
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getVendSupplirerNameAsync(_userInfo.CompanyId);

                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        destinationTypes.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmAccountNum,
                            Address = mzk.parmAddress,
                            RecID = mzk.parmRecID
                        });
                    }
                }
                destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                return destinationTypes;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return destinationTypes;
            }
        }
        async public System.Threading.Tasks.Task<IEnumerable<DestinationType>> GetDriversFromSvcAsync(string cusId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getDriversAsync(cusId, _userInfo.CompanyId);
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        destinationTypes.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.DestinationType
                        {
                            ContactName = mzk.parmName,
                            Id = mzk.parmDriverId,
                            RecID = mzk.parmRecID,
                            Address = mzk.parmAddress
                        });
                    }
                }
                return destinationTypes.OrderBy(o => o.ContactName);
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return null;
            }
        }
        async public System.Threading.Tasks.Task<List<Supplier>> FilterSuppliersByCriteria(string countryId, string provinceId, string cityId, string suburbId, string regionId)
        {
            List<Supplier> suplierList = new List<Supplier>();
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getVendorBySelectionAsync(_userInfo.CompanyId, countryId, provinceId, suburbId, cityId, regionId); ;

                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        suplierList.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.Supplier
                        {
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            AccountNum = mzk.parmAccountNum,
                            City = mzk.parmCityName,
                            Country = mzk.parmCountryName,
                            Province = mzk.parmStateName,
                            Suburb = mzk.parmSuburbanName
                        });
                    }
                }
                suplierList = suplierList.OrderBy(o => o.SupplierContactName).ToList<Supplier>();
                return suplierList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return suplierList;
            }
        }

        public async System.Threading.Tasks.Task<List<Supplier>> GetSuppliersByClass(string classId)
        {
            List<Supplier> suplierList = new List<Supplier>();
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return null;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.getVendorsByClassAsync(_userInfo.CompanyId, classId); ;

                if (result.response != null)
                {
                    foreach (var mzk in result.response.Where(x => x != null))
                    {
                        suplierList.Add(new Pithline.FMS.BusinessLogic.ServiceSchedule.Supplier
                        {
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            AccountNum = mzk.parmAccountNum,
                            City = mzk.parmCityName,
                            Country = mzk.parmCountryName,
                            Province = mzk.parmStateName,
                            Suburb = mzk.parmSuburbanName
                        });
                    }
                }
                suplierList = suplierList.OrderBy(o => o.SupplierContactName).ToList<Supplier>();
                return suplierList;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return suplierList;
            }
        }


        async public System.Threading.Tasks.Task<bool> UpdateConfirmationDatesToSvcAsync(long caseServiceRecId, ServiceSchedulingDetail serviceSchedulingDetail)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }
                var result = await client.updateConfirmationDatesAsync(caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                    parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2
                }, _userInfo.CompanyId);
                return result.response;
            }
            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }

        }
        async public System.Threading.Tasks.Task<bool> InsertServiceDetailsToSvcAsync(ServiceSchedulingDetail serviceSchedulingDetail, Address address, string caseNumber, long caseServiceRecId, long _entityRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var mzkAddressContract = new MzkAddressContract
                {
                    parmCity = address.SelectedCity != null ? address.SelectedCity.Id : string.Empty,
                    parmCountryRegionId = address.SelectedCountry != null ? address.SelectedCountry.Id : string.Empty,
                    parmProvince = address.SelectedProvince != null ? address.SelectedProvince.Id : string.Empty,
                    parmStreet = address.Street,
                    parmSubUrb = address.SelectedSuburb != null ? address.SelectedSuburb.Id : string.Empty,
                    parmZipCode = address.SelectedZip

                };
                var mzkServiceDetailsContract = new MzkServiceDetailsContract
                   {
                       parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                       parmAddress = serviceSchedulingDetail.Address,
                       parmEventDesc = serviceSchedulingDetail.EventDesc,
                       parmLiftLocationRecId = serviceSchedulingDetail.SelectedLocationType != null ? serviceSchedulingDetail.SelectedLocationType.RecID : default(long),
                       parmODOReading = serviceSchedulingDetail.ODOReading,
                       parmODOReadingDate = serviceSchedulingDetail.ODOReadingDate,
                       parmPreferredDateFirstOption = serviceSchedulingDetail.ServiceDateOption1,
                       parmPreferredDateSecondOption = serviceSchedulingDetail.ServiceDateOption2,
                       parmServiceType = serviceSchedulingDetail.SelectedServiceType,

                       parmSupplierId = serviceSchedulingDetail.SelectedDestinationType != null ? serviceSchedulingDetail.SelectedDestinationType.Id : string.Empty,
                       parmLiftRequired = serviceSchedulingDetail.IsLiftRequired == true ? NoYes.Yes : NoYes.No

                   };
                if (serviceSchedulingDetail.IsLiftRequired && serviceSchedulingDetail.SelectedLocationType !=null)
                {
                    mzkServiceDetailsContract.parmLocationType = new MzkLocationTypeContract
                    {
                        parmLocationType = (EXDCaseServiceDestinationType)Enum.Parse(typeof(EXDCaseServiceDestinationType), serviceSchedulingDetail.SelectedLocationType.LocType)
                    };
                }

                var result = await client.insertServiceDetailsAsync(caseNumber, caseServiceRecId, _entityRecId, mzkServiceDetailsContract
                      , mzkAddressContract, _userInfo.CompanyId);

                if (serviceSchedulingDetail.ODOReadingSnapshot != null && !string.IsNullOrEmpty(serviceSchedulingDetail.ODOReadingSnapshot.ImageBinary))
                {
                    await client.saveImageAsync(new ObservableCollection<Mzk_ImageContract>{new Mzk_ImageContract
                {
                     parmCaseNumber = caseNumber,
                      parmFileName = "ServiceScheduling_ODOReading.png",
                       parmImageData = serviceSchedulingDetail.ODOReadingSnapshot.ImageBinary
                }});
                }

                return result.response;
            }

            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }
        }
        async public System.Threading.Tasks.Task<bool> InsertSelectedSupplierToSvcAsync(SupplierSelection supplierSelection, string caseNumber, long caseServiceRecId)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || supplierSelection.SelectedSupplier == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return false;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                var result = await client.insertVendDetAsync(caseNumber, caseServiceRecId, default(long), new MzkServiceDetailsContract
                {
                    parmContactPersonName = supplierSelection.SelectedSupplier.SupplierContactName,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                    parmSupplierId = supplierSelection.SelectedSupplier.AccountNum
                }, new MzkAddressContract(), _userInfo.CompanyId);

                return result != null;
            }


            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return false;
            }
        }

        async public System.Threading.Tasks.Task<string> UpdateStatusListToSvcAsync(DriverTask task)
        {
            try
            {
                var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                if (connectionProfile == null || connectionProfile.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
                    return task.Status;

                if (_userInfo == null)
                {
                    _userInfo = JsonConvert.DeserializeObject<UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo].ToString());
                }

                ObservableCollection<MzkServiceSchdTasksContract> mzkTasks = new ObservableCollection<MzkServiceSchdTasksContract>();

                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                actionStepMapping.Add(Pithline.FMS.BusinessLogic.Helpers.DriverTaskStatus.AwaitServiceBookingDetail, EEPActionStep.MaintenceServiceSheduling);
                actionStepMapping.Add(Pithline.FMS.BusinessLogic.Helpers.DriverTaskStatus.AwaitSupplierSelection, EEPActionStep.SelectSupplier);
                actionStepMapping.Add(Pithline.FMS.BusinessLogic.Helpers.DriverTaskStatus.AwaitServiceBookingConfirmation, EEPActionStep.ServiceSchedulling);
                mzkTasks.Add(new MzkServiceSchdTasksContract
                {
                    parmCaseID = task.CaseNumber,
                    parmCaseCategory = task.CaseCategory,
                    parmCaseServiceRecID = task.CaseServiceRecID,
                    parmStatus = task.Status,
                    parmServiceRecID = task.ServiceRecID,
                    parmStatusDueDate = task.StatusDueDate,
                    parmEEPActionStep = actionStepMapping[task.Status]
                });
                var result = await client.updateStatusListAsync(mzkTasks, _userInfo.CompanyId);

                return result.response.FirstOrDefault().parmStatus;
            }

            catch (Exception ex)
            {
                AppSettings.Instance.ErrorMessage = ex.Message;
                return task.Status;
            }
        }
    }
}