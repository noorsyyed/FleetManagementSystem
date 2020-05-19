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
    public class LocationService : ILocationService
    {
        IHttpFactory _httpFactory;
        public LocationService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        async public Task<ObservableCollection<BusinessLogic.Portable.SSModels.Country>> GetCountryList(BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetCountryList", JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<BusinessLogic.Portable.SSModels.Country>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

            }
            return null;

        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Province>> GetProvinceList(string countryId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetProvinceList", countryId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<BusinessLogic.Portable.SSModels.Province>>(await response.Content.ReadAsStringAsync());

                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

            }
            return null;

        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.City>> GetCityList(string countryId, string stateId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetCityList", countryId, stateId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<BusinessLogic.Portable.SSModels.City>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

            }
            return null;

        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Suburb>> GetSuburbList(string countryId, string stateId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetSuburbList", countryId, stateId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<BusinessLogic.Portable.SSModels.Suburb>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

            }
            return null;

        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Region>> GetRegionList(string countryId, string stateId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetRegionList", countryId, stateId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<BusinessLogic.Portable.SSModels.Region>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();


            }
            return null;

        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<string>> GetZipcodeList(string countryId, string stateId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetZipCodeList", countryId, stateId, JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<ObservableCollection<string>>(await response.Content.ReadAsStringAsync());

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
