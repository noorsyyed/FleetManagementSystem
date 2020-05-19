using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services
{

    public class TaskService : ITaskService
    {
        IHttpFactory _httpFactory;
        public TaskService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }



        async public Task<ObservableCollection<TITask>> GetTasksAsync(string userId, string companyId)
        {
            try
            {

                var postData = new { target = "TechnicalInspection", parameters = new[] { "GetTasks", userId, companyId } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<TITask>>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
                return null;
            }
        }

        async public Task<bool> InsertInspectionDataAsync(List<TIData> tiData, BusinessLogic.Portable.TIModels.Task task, List<ImageCapture> imageCaptureList, string companyId)
        {
            try
            {
                var postData = new { target = "TechnicalInspection", method = "save", parameters = new[] { "InsertInspectionData", Newtonsoft.Json.JsonConvert.SerializeObject(tiData), JsonConvert.SerializeObject(task), Newtonsoft.Json.JsonConvert.SerializeObject(imageCaptureList), companyId } };
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
    }

}
