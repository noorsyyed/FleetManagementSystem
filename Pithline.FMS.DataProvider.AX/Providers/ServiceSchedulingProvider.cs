using Pithline.FMS.DataProvider.AX.Helpers;
using Pithline.FMS.DataProvider.AX.SSModels;
using Pithline.FMS.DataProvider.AX.SSProxy;
using Pithline.FMS.Framework.Web.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Pithline.FMS.DataProvider.AX.Providers
{
    [DataProvider(Name = "ServiceScheduling")]
    public class ServiceSchedulingProvider : IDataProvider
    {
        MzkServiceSchedulingServiceClient _client;

        public System.Collections.IList GetDataList(object[] criterias)
        {
            try
            {
                GetService();
                System.Collections.IList response = null;
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.GetTasks:
                        response = GetTasks(JsonConvert.DeserializeObject<UserInfo>(criterias[1].ToString()));
                        break;

                    case ActionSwitch.GetCountryList:
                        response = GetCountryList(JsonConvert.DeserializeObject<UserInfo>(criterias[1].ToString()));
                        break;

                    case ActionSwitch.GetProvinceList:
                        response = GetProvinceList(criterias[1].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.GetCityList:
                        response = GetCityList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetSuburbList:
                        response = GetSuburbList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetRegionList:
                        response = GetRegionList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetZipcodeList:
                        response = GetZipcodeList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetDestinationTypeList:
                        response = GetDestinationTypeList(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.FilterSuppliersByCriteria:
                        response = FilterSuppliersByCriteria(criterias[1].ToString(), criterias[2].ToString(), criterias[3].ToString(), criterias[4].ToString(), criterias[5].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[6].ToString()));
                        break;
                    case ActionSwitch.FilterSuppliersByGeoLocation:
                        response = FilterSuppliersByGeoLocation(criterias[1].ToString(), criterias[2].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.GetSupplierByClass:
                        response = GetSuppliersByClass(criterias[1].ToString(), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;
                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public object GetSingleData(object[] criterias)
        {
            try
            {
                object response = null;
                GetService();
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.ValidateUser:
                        response = ValidateUser(criterias[2].ToString(), criterias[3].ToString());
                        break;

                    case ActionSwitch.GetServiceDetails:
                        response = GetServiceDetails(criterias[1].ToString(), long.Parse(criterias[2].ToString()), long.Parse(criterias[3].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[4].ToString()));
                        break;

                    case ActionSwitch.GetUserInfo:
                        response = GetUserInfo(criterias[1].ToString());
                        break;
                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public bool DeleteData(object[] criterias)
        {
            throw new NotImplementedException();
        }

        public object SaveData(object[] criterias)
        {
            try
            {
                object response = null;
                GetService();
                switch (criterias[0].ToString())
                {
                    case ActionSwitch.InsertServiceDetails:
                        response = InsertServiceDetails(JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[1].ToString()), JsonConvert.DeserializeObject<Address>(criterias[2].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.InsertSelectedSupplier:
                        response = InsertSelectedSupplier(JsonConvert.DeserializeObject<SupplierSelection>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.InsertConfirmedServiceDetail:
                        response = InsertConfirmedServiceDetail(JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;

                    case ActionSwitch.UpdateConfirmationDates:
                        response = UpdateConfirmationDates(long.Parse(criterias[1].ToString()), JsonConvert.DeserializeObject<ServiceSchedulingDetail>(criterias[2].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[3].ToString()));
                        break;

                    case ActionSwitch.UpdateStatusList:
                        response = UpdateStatusList(JsonConvert.DeserializeObject<Pithline.FMS.DataProvider.AX.SSModels.Task>(criterias[1].ToString()), JsonConvert.DeserializeObject<UserInfo>(criterias[2].ToString()));
                        break;
                }
                _client.Close();
                return response;
            }
            catch (Exception)
            {
                _client.Close();
                throw;
            }
        }

        public object GetService()
        {
            return GetServiceClient();
        }

        private MzkServiceSchedulingServiceClient GetServiceClient()
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://SRFMLAXTEST01/MicrosoftDynamicsAXAif60/SSService/xppservice.svc
                _client = new MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/SSService/xppservice.svc?wsdl"));
                _client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "erpsetup";
                _client.ClientCredentials.UserName.Password = "AXrocks100";
                _client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                _client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("erpsetup", "AXrocks100", "lfmd");
            }
            catch (Exception)
            {
                throw;
            }

            return _client;
        }

        private bool ValidateUser(string userId, string password)
        {
            try
            {
                return !_client.validateUser(new CallContext() { }, userId, password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private UserInfo GetUserInfo(string userId)
        {
            try
            {
                var result = _client.getUserDetails(new CallContext() { }, userId);
                if (result != null)
                {
                    return new UserInfo
                    {
                        UserId = result.parmUserID,
                        CompanyId = result.parmCompany,
                        CompanyName = result.parmCompanyName,
                        Name = result.parmUserName
                    };
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Pithline.FMS.DataProvider.AX.SSModels.Task> GetTasks(UserInfo userInfo)
        {
            try
            {

                var result = _client.getTasks(new CallContext() { }, userInfo.UserId, userInfo.CompanyId);
                List<Pithline.FMS.DataProvider.AX.SSModels.Task> driverTaskList = new List<Pithline.FMS.DataProvider.AX.SSModels.Task>();
                if (result != null)
                {

                    foreach (var mzkTask in result.Reverse())
                    {
                        var startTime = new DateTime(mzkTask.parmConfirmationDate.Year, mzkTask.parmConfirmationDate.Month, mzkTask.parmConfirmationDate.Day);

                        driverTaskList.Add(new Pithline.FMS.DataProvider.AX.SSModels.Task
                        {
                            CaseNumber = mzkTask.parmCaseID,
                            Address = Regex.Replace(mzkTask.parmContactPersonAddress, "\n", ","),
                            CustomerName = mzkTask.parmCustName,
                            CustPhone = string.IsNullOrEmpty(mzkTask.parmContactPersonPhone) ? "" : "+" + mzkTask.parmContactPersonPhone,
                            Status = mzkTask.parmStatus,
                            StatusDueDate = mzkTask.parmStatusDueDate.ToShortDateString(),
                            RegistrationNumber = mzkTask.parmRegistrationNum,
                            AllocatedTo = userInfo.Name,
                            UserId = mzkTask.parmUserID,
                            CaseCategory = mzkTask.parmCaseCategory,
                            CaseServiceRecID = mzkTask.parmCaseServiceRecID,
                            DriverFirstName = mzkTask.parmDriverFirstName,
                            DriverLastName = mzkTask.parmDriverLastName,
                            DriverPhone = mzkTask.parmDriverPhone,
                            Make = mzkTask.parmMake,
                            Model = mzkTask.parmModel,
                            Description = mzkTask.parmVehicleDescription,
                            CustEmailId = mzkTask.parmEmail,
                            ServiceRecID = mzkTask.parmServiceRecID,
                            CustomerId = mzkTask.parmCustAccount,
                            ConfirmedDate = mzkTask.parmConfirmationDate.ToShortDateString(),
                            ContactName = mzkTask.parmContactPersonName,
                            AppointmentStart = mzkTask.parmStatus == Pithline.FMS.DataProvider.AX.Helpers.TaskStatus.Completed ? startTime : DateTime.MinValue,
                            AppointmentEnd = mzkTask.parmStatus == Pithline.FMS.DataProvider.AX.Helpers.TaskStatus.Completed ? startTime.AddHours(24) : DateTime.MinValue,
                            VehicleClassId = mzkTask.parmVehicleClassId,
                            VehicleSubClassId = mzkTask.parmVehicleSubClassId,
                            
                            
                        });
                    }
                }
               
                return driverTaskList.OrderByDescending(x => x.CaseNumber).Where(x=>x.Status != TaskStatus.AwaitServiceConfirmation).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Country> GetCountryList(UserInfo userInfo)
        {
            try
            {

                var result = _client.getCountryRegionList(new CallContext() { }, userInfo.CompanyId);
                List<Country> countryList = new List<Country>();
                if (result != null)
                {

                    foreach (var mzk in result.OrderBy(o => o.parmCountryRegionName))
                    {
                        countryList.Add(
                                           new Country
                                           {
                                               Name = mzk.parmCountryRegionName,
                                               Id = mzk.parmCountryRegionId
                                           }
                                           );
                    };
                }

                return countryList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Province> GetProvinceList(string countryId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getProvinceList(new CallContext() { }, countryId, userInfo.CompanyId);
                List<Province> provinceList = new List<Province>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        provinceList.Add(new Province { Name = mzk.parmStateName, Id = mzk.parmStateId });
                    }
                }
                return provinceList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<City> GetCityList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getCityList(new CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<City> cityList = new List<City>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        cityList.Add(new City { Name = mzk.parmCountyId, Id = mzk.parmCountyId });
                    }
                }
                return cityList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Suburb> GetSuburbList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getSuburbList(new CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<Suburb> suburbList = new List<Suburb>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suburbList.Add(new Suburb { Name = mzk.parmCity, Id = mzk.parmCity });
                    }
                }
                return suburbList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<Region> GetRegionList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getRegions(new CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<Region> regionList = new List<Region>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        regionList.Add(new Region { Name = mzk.parmRegionName, Id = mzk.parmRegion });
                    }
                }
                return regionList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<string> GetZipcodeList(string countryId, string stateId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getZipcodeList(new CallContext() { }, countryId, stateId, userInfo.CompanyId);
                List<string> zipcodeList = new List<string>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        zipcodeList.Add(mzk.parmZipCode);
                    }
                }
                return zipcodeList;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private ServiceSchedulingDetail GetServiceDetails(string caseNumber, long caseServiceRecId, long serviceRecId, UserInfo userInfo)
        {
            try
            {

                var result = _client.getServiceDetails(new CallContext() { }, caseNumber, caseServiceRecId, userInfo.CompanyId);
                ServiceSchedulingDetail detailServiceScheduling = null;
                if (result != null)
                {

                    foreach (var mzk in result)
                    {
                        detailServiceScheduling = (new ServiceSchedulingDetail
                        {
                            Address = mzk.parmAddress,
                            AdditionalWork = mzk.parmAdditionalWork,
                            ServiceDateOption1 = mzk.parmPreferredDateFirstOption.Year == 1900 ? string.Empty : mzk.parmPreferredDateFirstOption.ToString("MM/dd/yyyy HH:mm"),
                            ServiceDateOption2 = mzk.parmPreferredDateSecondOption.Year == 1900 ? string.Empty : mzk.parmPreferredDateSecondOption.ToString("MM/dd/yyyy HH:mm"),
                            ODOReading = Int64.Parse(mzk.parmODOReading.ToString()),
                            ODOReadingDate = mzk.parmODOReadingDate.Year == 1900 ? string.Empty : mzk.parmODOReadingDate.ToString("MM/dd/yyyy HH:mm"),
                            ServiceType = GetServiceTypes(caseNumber, userInfo.CompanyId),
                            LocationTypes = GetLocationType(serviceRecId, userInfo.CompanyId),
                            SupplierName = mzk.parmSupplierName,
                            EventDesc = mzk.parmEventDesc,
                            ContactPersonName = mzk.parmContactPersonName,
                            ContactPersonPhone = mzk.parmContactPersonPhone,
                            SupplierDateTime = DateTime.Now,// need to add in service
                            CaseNumber = caseNumber,
                            CaseServiceRecID = caseServiceRecId,
                            ODOReadingSnapshot = mzk.parmODOReadingImage,
                            //  SelectedLocationType = mzk.parmLocationType,
                            SelectedServiceType = mzk.parmServiceType,
                            IsLiftRequired = mzk.parmLiftRequired == NoYes.Yes ? true : false,
                            AccountNumber = mzk.parmSupplierId,
                            ConfirmedDate = mzk.parmConfirmedDate.Year == 1900 ? "" : mzk.parmConfirmedDate.ToString("MM/dd/yyyy HH:mm")

                        });
                        detailServiceScheduling.SelectedLocType = detailServiceScheduling.LocationTypes.Find(x => x.RecID == mzk.parmLocationType.parmRecID);
                    }

                }

                return detailServiceScheduling;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<string> GetServiceTypes(string caseNumber, string companyId)
        {

            var result = _client.getServiceTypes(new CallContext() { }, caseNumber, companyId);
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
        private List<LocationType> GetLocationType(long serviceRecId, string companyId)
        {
            try
            {
                var result = _client.getLocationType(new CallContext() { }, serviceRecId, companyId);
                List<LocationType> locationTypes = new List<LocationType>();
                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        locationTypes.Add(new LocationType
                        {
                            LocationName = mzk.parmLocationName,
                            LocType = mzk.parmLocationType.ToString(),
                            RecID = mzk.parmRecID,
                        });
                    }
                }
                return locationTypes;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<DestinationType> GetDestinationTypeList(string callerKey, string cusId, UserInfo userInfo)
        {
            try
            {
                List<DestinationType> destinationTypes = new List<DestinationType>();
                if (callerKey.Contains("Customer"))
                {
                    var result = _client.getCustomers(new CallContext() { }, cusId, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmAccountNum,
                                RecID = mzk.parmRecID,
                                Address = mzk.parmAddress
                            });
                        }
                    }
                    destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();

                }


                if (callerKey.Contains("Supplier") || callerKey.Contains("Vendor"))
                {
                    var result = _client.getVendSupplirerName(new CallContext() { }, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmAccountNum,
                                Address = mzk.parmAddress,
                                RecID = mzk.parmRecID

                            });
                        }
                    }
                    destinationTypes = destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();
                }


                if (callerKey.Contains("Driver"))
                {
                    var result = _client.getDrivers(new CallContext() { }, cusId, userInfo.CompanyId);

                    if (result != null)
                    {
                        foreach (var mzk in result)
                        {
                            destinationTypes.Add(new DestinationType
                            {
                                ContactName = mzk.parmName,
                                Id = mzk.parmDriverId,
                                RecID = mzk.parmRecID,
                                Address = mzk.parmAddress
                            });
                        }
                    }

                }

                return destinationTypes.OrderBy(o => o.ContactName).ToList<DestinationType>();

            }
            catch (Exception)
            {
                throw;
            }

        }

        private bool UpdateConfirmationDates(long caseServiceRecId, ServiceSchedulingDetail serviceSchedulingDetail, UserInfo userInfo)
        {
            try
            {

                var result = _client.updateConfirmationDates(new CallContext() { }, caseServiceRecId, new MzkServiceDetailsContract
                {
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2)
                }, userInfo.CompanyId);
                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }
        private bool InsertServiceDetails(ServiceSchedulingDetail serviceSchedulingDetail, Address address, UserInfo userInfo)
        {
            try
            {

                var mzkAddressContract = new MzkAddressContract
                {
                    parmCity = address.SelectedCity != null ? address.SelectedCity : string.Empty,
                    parmCountryRegionId = address.SelectedCountry != null ? address.SelectedCountry : string.Empty,
                    parmProvince = address.Selectedprovince != null ? address.Selectedprovince : string.Empty,
                    parmStreet = address.Street,
                    parmSubUrb = address.SelectedSuburb != null ? address.SelectedSuburb : string.Empty,
                    parmZipCode = address.SelectedZip

                };
                var mzkServiceDetailsContract = new MzkServiceDetailsContract
                {

                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,

                    parmODOReading = serviceSchedulingDetail.ODOReading.ToString(),
                    parmODOReadingDate = DateTime.Parse(serviceSchedulingDetail.ODOReadingDate, new CultureInfo("en-US").DateTimeFormat),
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1, new CultureInfo("en-US").DateTimeFormat),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2, new CultureInfo("en-US").DateTimeFormat),
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmLiftLocationRecId = serviceSchedulingDetail.SelectedLocType == null ? default(long) : serviceSchedulingDetail.SelectedLocType.RecID,
                    parmSupplierId = serviceSchedulingDetail.SelectedDestinationType == null ? string.Empty : serviceSchedulingDetail.SelectedDestinationType.Id,


                    parmLiftRequired = serviceSchedulingDetail.IsLiftRequired == true ? NoYes.Yes : NoYes.No
                };

                if (serviceSchedulingDetail.SelectedLocType != null)
                {
                    mzkServiceDetailsContract.parmLocationType = new MzkLocationTypeContract
                               {
                                   parmLocationType = (EXDCaseServiceDestinationType)Enum.Parse(typeof(EXDCaseServiceDestinationType), serviceSchedulingDetail.SelectedLocType.LocType)
                               };
                }
                var recID = serviceSchedulingDetail.SelectedDestinationType == null ? default(long) : serviceSchedulingDetail.SelectedDestinationType.RecID;

                var result = _client.insertServiceDetails(new CallContext() { }, serviceSchedulingDetail.CaseNumber, serviceSchedulingDetail.CaseServiceRecID, recID, mzkServiceDetailsContract
                      , mzkAddressContract, userInfo.CompanyId);

                if (!string.IsNullOrEmpty(serviceSchedulingDetail.ODOReadingSnapshot))
                {
                    _client.saveImage(new CallContext { }, new Mzk_ImageContract[]{new Mzk_ImageContract
                {
                     parmCaseNumber = serviceSchedulingDetail.CaseNumber,
                      parmFileName = "ServiceScheduling_ODOReading.png",
                       parmImageData = serviceSchedulingDetail.ODOReadingSnapshot
                }});
                }
                return result;

            }

            catch (Exception)
            {
                throw;
            }
        }



        public List<Supplier> FilterSuppliersByGeoLocation(string countryName, string cityName, UserInfo userInfo)
        {
            try
            {
                var result = _client.getVendorByName(new CallContext() { }, countryName, cityName, userInfo.CompanyId);

                List<Supplier> suppliers = new List<Supplier>();

                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suppliers.Add(new Supplier
                        {
                            AccountNum = mzk.parmAccountNum,
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            Country = mzk.parmCountry,
                            Province = mzk.parmState,
                            City = mzk.parmCityName,
                            Suburb = mzk.parmSuburban,
                            Email = mzk.parmEmail,
                            Address = mzk.parmAddress,
                            CountryName = mzk.parmCountryName,
                            ProvinceName = mzk.parmStateName,
                            CityName = mzk.parmCityName,
                        });
                    }
                }

                return suppliers;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Supplier> FilterSuppliersByCriteria(string countryId, string provinceId, string cityId, string suburbId, string regionId, UserInfo userInfo)
        {

            try
            {
                var result = _client.getVendorBySelection(new CallContext() { }, userInfo.CompanyId, countryId, provinceId, suburbId, cityId, regionId);

                List<Supplier> suppliers = new List<Supplier>();

                if (result != null)
                {
                    foreach (var mzk in result)
                    {
                        suppliers.Add(new Supplier
                        {
                            AccountNum = mzk.parmAccountNum,
                            SupplierContactName = mzk.parmContactPersonName,
                            SupplierContactNumber = mzk.parmContactPersonPhone,
                            SupplierName = mzk.parmName,
                            Country = mzk.parmCountry,
                            Province = mzk.parmState,
                            City = mzk.parmCityName,
                            Suburb = mzk.parmSuburban,
                            Email = mzk.parmEmail,
                            Address = mzk.parmAddress,
                            CountryName = mzk.parmCountryName,
                            ProvinceName = mzk.parmStateName,
                            CityName = mzk.parmCityName,
                        });
                    }
                }
                return suppliers.OrderBy(o => o.SupplierName).ToList<Supplier>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Supplier> GetSuppliersByClass(string classId, UserInfo userInfo)
        {
            List<Supplier> suplierList = new List<Supplier>();
            try
            {

                var result = _client.getVendorsByClass(new CallContext { Company = userInfo.CompanyId }, userInfo.CompanyId, classId); ;

                if (result != null)
                {
                    foreach (var mzk in result.Where(x => x != null))
                    {
                        suplierList.Add(new Supplier
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
                suplierList = suplierList.OrderBy(o => o.SupplierName).ToList<Supplier>();
                return suplierList;
            }
            catch (Exception ex)
            {

                return suplierList;
            }
        }

        private bool InsertSelectedSupplier(SupplierSelection supplierSelection, UserInfo userInfo)
        {
            try
            {

                _client.insertVendDet(new CallContext() { }, supplierSelection.CaseNumber, supplierSelection.CaseServiceRecID, default(long), new MzkServiceDetailsContract
                {
                    parmContactPersonName = supplierSelection.SelectedSupplier.SupplierContactName,
                    parmSupplierName = supplierSelection.SelectedSupplier.SupplierName,
                    parmContactPersonPhone = supplierSelection.SelectedSupplier.SupplierContactNumber,
                    parmSupplierId = supplierSelection.SelectedSupplier.AccountNum
                }, new MzkAddressContract(), userInfo.CompanyId);

                return true;

            }

            catch (Exception)
            {
                throw;
            }
        }
        private bool InsertConfirmedServiceDetail(ServiceSchedulingDetail serviceSchedulingDetail, UserInfo userInfo)
        {
            try
            {

                var result = _client.insertServiceDetails(new CallContext() { }, serviceSchedulingDetail.CaseNumber, serviceSchedulingDetail.CaseServiceRecID, default(long), new MzkServiceDetailsContract
                {
                    parmAdditionalWork = serviceSchedulingDetail.AdditionalWork,
                    parmAddress = serviceSchedulingDetail.Address,
                    parmEventDesc = serviceSchedulingDetail.EventDesc,
                    parmLocationType = new MzkLocationTypeContract
                    {
                        parmLocationType = (EXDCaseServiceDestinationType)Enum.Parse(typeof(EXDCaseServiceDestinationType), serviceSchedulingDetail.SelectedLocationType.LocType)
                    },

                    parmODOReading = serviceSchedulingDetail.ODOReading.ToString(),
                    parmODOReadingDate = DateTime.Parse(serviceSchedulingDetail.ODOReadingDate),
                    parmPreferredDateFirstOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption1),
                    parmPreferredDateSecondOption = DateTime.Parse(serviceSchedulingDetail.ServiceDateOption2),
                    parmServiceType = serviceSchedulingDetail.SelectedServiceType,
                    parmSupplierName = serviceSchedulingDetail.SupplierName,
                    parmContactPersonName = serviceSchedulingDetail.ContactPersonName,
                    parmContactPersonPhone = serviceSchedulingDetail.ContactPersonPhone,

                }, new MzkAddressContract(), userInfo.CompanyId);


                return result;

            }

            catch (Exception)
            {
                throw;
            }
        }
        private CaseStatus UpdateStatusList(Pithline.FMS.DataProvider.AX.SSModels.Task task, UserInfo userInfo)
        {
            try
            {

                ObservableCollection<MzkServiceSchdTasksContract> mzkTasks = new ObservableCollection<MzkServiceSchdTasksContract>();

                Dictionary<string, EEPActionStep> actionStepMapping = new Dictionary<string, EEPActionStep>();

                actionStepMapping.Add(DriverTaskStatus.AwaitServiceBookingDetail, EEPActionStep.MaintenceServiceSheduling);
                actionStepMapping.Add(DriverTaskStatus.AwaitSupplierSelection, EEPActionStep.SelectSupplier);
                actionStepMapping.Add(DriverTaskStatus.AwaitServiceBookingConfirmation, EEPActionStep.ServiceSchedulling);

                mzkTasks.Add(new MzkServiceSchdTasksContract
                {
                    parmCaseID = task.CaseNumber,
                    parmCaseCategory = task.CaseCategory,
                    parmCaseServiceRecID = task.CaseServiceRecID,
                    parmStatus = task.Status,
                    parmServiceRecID = task.ServiceRecID,
                    parmStatusDueDate = DateTime.Parse(task.StatusDueDate),
                    parmEEPActionStep = actionStepMapping[task.Status]
                });
                var result = _client.updateStatusList(new CallContext() { }, mzkTasks.ToArray(), userInfo.CompanyId);

                return new CaseStatus { Status = result.FirstOrDefault().parmStatus };

            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
