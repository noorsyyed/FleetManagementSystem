using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.ServiceScheduling.UILogic.Portable.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Services
{
    public class ServiceDetailService : IServiceDetailService
    {
        IHttpFactory _httpFactory;
        public ServiceDetailService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<bool> InsertServiceDetailsAsync(BusinessLogic.Portable.SSModels.ServiceSchedulingDetail serviceSchedulingDetail, BusinessLogic.Portable.SSModels.Address address, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {

                var postData = new { target = "ServiceScheduling", method = "save", parameters = new[] { "InsertServiceDetails", JsonConvert.SerializeObject(serviceSchedulingDetail), JsonConvert.SerializeObject(address), JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

                }
            }
            catch (Exception ex)
            {
                   new MessageDialog(ex.Message).ShowAsync();
             
            }
            return false;
        }

        public async Task<BusinessLogic.Portable.SSModels.ServiceSchedulingDetail> GetServiceDetailAsync(string caseNumber, long caseServiceRecId, long serviceRecId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", method = "single", parameters = new[] { "GetServiceDetails", caseNumber, caseServiceRecId.ToString(), serviceRecId.ToString(), JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ServiceSchedulingDetail>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                   new MessageDialog(ex.Message).ShowAsync();
             
            }
            return null;

        }


        public Task<System.Collections.ObjectModel.ObservableCollection<string>> GetServiceTypes(string caseNumber, string companyId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<LocationType>> GetLocationType(long serviceRecId, string companyId)
        {
            throw new NotImplementedException();
        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<DestinationType>> GetDestinationTypeList(string callerKey, string cusId, UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetDestinationTypeList", callerKey, cusId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<DestinationType>>(await response.Content.ReadAsStringAsync());

                }
            }
            catch (Exception ex)
            {
                   new MessageDialog(ex.Message).ShowAsync();
            
            }

            return null;

        }
    }
}
