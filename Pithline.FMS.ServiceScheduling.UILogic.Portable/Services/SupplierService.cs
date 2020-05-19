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
    public class SupplierService : ISupplierService
    {
        IHttpFactory _httpFactory;
        public SupplierService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Supplier>> GetSuppliersByClassAsync(string classId, BusinessLogic.Portable.SSModels.UserInfo userInfo)
        {
            try
            {

                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetSuppliersByClass", classId, Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

            }
            return null;

        }




        public async Task<bool> InsertSelectedSupplierAsync(SupplierSelection supplierSelection, UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", method = "save", parameters = new[] { "InsertSelectedSupplier", Newtonsoft.Json.JsonConvert.SerializeObject(supplierSelection), Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
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

        public async Task<ObservableCollection<Supplier>> SearchSupplierByLocationAsync(string countryId, string provinceId, string cityId, string suburbId, string regionId, UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", parameters = new[] { "FilterSuppliersByCriteria", countryId, provinceId, cityId, suburbId, regionId, Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<Supplier>>(await response.Content.ReadAsStringAsync());
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
