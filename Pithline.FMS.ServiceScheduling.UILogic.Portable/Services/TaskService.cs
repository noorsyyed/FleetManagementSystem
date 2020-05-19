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

    public class TaskService : ITaskService
    {
        IHttpFactory _httpFactory;
        public TaskService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        async public Task<System.Collections.ObjectModel.ObservableCollection<BusinessLogic.Portable.SSModels.Task>> GetTasksAsync(UserInfo userInfo)
        {
            try
            {

                var postData = new { target = "ServiceScheduling", parameters = new[] { "GetTasks", Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<ObservableCollection<Pithline.FMS.BusinessLogic.Portable.SSModels.Task>>(await response.Content.ReadAsStringAsync());
                }
                return null;
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
                return null;
            }


        }



        async public Task<BusinessLogic.Portable.SSModels.CaseStatus> UpdateStatusListAsync(BusinessLogic.Portable.SSModels.Task task, UserInfo userInfo)
        {
            try
            {
                var postData = new { target = "ServiceScheduling", method = "save", parameters = new[] { "UpdateStatusList", JsonConvert.SerializeObject(task), Newtonsoft.Json.JsonConvert.SerializeObject(userInfo) } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {

                    return JsonConvert.DeserializeObject<CaseStatus>(await response.Content.ReadAsStringAsync());
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
