using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.ServiceModel;
using Pithline.FMS.VehicleInspection.BackgroundTask.VIProxy;
using Windows.Security.Credentials;
using Windows.Storage;


namespace Pithline.FMS.VehicleInspection.BackgroundTask
{
    public sealed class SilentSync : IBackgroundTask
    {
        private MzkVehicleInspectionServiceClient _client;
        private UserInfo _userInfo;
        private const string worldWeatherAPI = "http://api.worldweatheronline.com/free/v1/weather.ashx?format=json&num_of_days=1&key=n5v69mv2e2kmyq93u2m494wt&q=";
        async public void Run(IBackgroundTaskInstance taskInstance)
        {
          string PasswordVaultResourceName = "VehicleInspection";
          
            ConnectAsync("axbcsvc", "AXrocks100");
            var cred = this.GetSavedCredentials(PasswordVaultResourceName);
            if (cred != null)
            {
                cred.RetrievePassword();
                var res = await this.GetUserInfoAsync(cred.UserName);
                if (res != null && res.response != null)
                {
                    _userInfo = new UserInfo
                    {
                        UserId = res.response.parmUserID,
                        CompanyId = res.response.parmCompany,
                        CompanyName = res.response.parmCompanyName,
                        Name = res.response.parmUserName
                    };
                }
                
                UpdateTile(await GetTasksAsync());
                //await GetWeatherInfoAsync();
            }
        }

        async private System.Threading.Tasks.Task<MzkVehicleInspectionServiceGetUserDetailsResponse> GetUserInfoAsync(string userId)
        {
            try
            {
                return await _client.getUserDetailsAsync(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private PasswordCredential GetSavedCredentials(string resource)
        {
            try
            {
                var vault = new PasswordVault();
                var credentials = vault.FindAllByResource(resource);
                var cred = credentials.FirstOrDefault();
                if (cred != null)
                    return vault.Retrieve(resource, cred.UserName);
                else
                    return null;
            }
            // The password vault throws System.Exception if no credentials have been stored with this resource.
            catch (Exception)
            {
                return null;
            }
        }

        async private System.Threading.Tasks.Task<List<VITask>> GetTasksAsync()
         {
             var tasks = new List<VITask>();
             var result = await _client.getTasksAsync(_userInfo.CompanyId, _userInfo.UserId);
             foreach (var task in result.response)
             {
                 tasks.Add(new VITask
                 {
                     CaseNumber = task.parmCaseID,
                     Customer = task.parmCustName,
                     Address = task.parmContactPersonAddress
                 });
             }
             return tasks;
         }


        private VIProxy.MzkVehicleInspectionServiceClient ConnectAsync(string userName, string password, string domain = "lfmd")
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;//http://srfmlaxtest01/MicrosoftDynamicsAXAif60/VehicleInspection/xppservice.svc
                _client = new VIProxy.MzkVehicleInspectionServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/VehicleInspection/xppservice.svc"));
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
        private static void UpdateTile(List<VITask> feed)
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications. 
            int itemCount = 0;

            // Create a tile notification for each feed item.
            foreach (var item in feed.Take(3))
            {
                XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310TextList03);

                
              
                tileXml.GetElementById("1").InnerText = item.CaseNumber;
                tileXml.GetElementById("2").InnerText = item.Customer;


                // Create a new tile notification. 
                updater.Update(new TileNotification(tileXml));

            }
        }


        private static async System.Threading.Tasks.Task GetWeatherInfoAsync()
        {
            var request = (HttpWebRequest)WebRequest.Create(worldWeatherAPI + "hyderabad,India");
            var response = (HttpWebResponse)await request.GetResponseAsync();
            JsonSerializer serializer = new JsonSerializer();
            var reader = new JsonTextReader(new StreamReader(response.GetResponseStream()));
            var weatherOjb = JObject.Parse(serializer.Deserialize(reader).ToString());
            var currentCondition = weatherOjb["data"]["current_condition"];
            var weather = (from item in currentCondition
                           select new WeatherInfo
                           {
                               CloudCover = item["cloudcover"].ToString(),
                               Humidity = item["humidity"].ToString(),
                               PrecipMM = item["precipMM"].ToString(),
                               Temp_C = item["temp_C"].ToString(),
                               Temp_F = item["temp_F"].ToString(),
                               WeatherIconUrl = item["weatherIconUrl"][0]["value"].ToString(),
                               WeatherDesc = item["weatherDesc"][0]["value"].ToString(),
                           }).First();

            await SqliteHelper.Storage.DropTableAsync<WeatherInfo>();
            await SqliteHelper.Storage.CreateTableAsync<WeatherInfo>();
            await SqliteHelper.Storage.InsertSingleRecordAsync(weather);

            //VIServiceProxy.VehicleInspectionServiceClient client = new VIServiceProxy.VehicleInspectionServiceClient();
            //client.createAccessoriesAsync(new System.Collections.ObjectModel.ObservableCollection<VIServiceProxy.MzkAccessoryContract>
            //{
            //   new MzkAccessoryContract{  parmVehicleInsRecID = 5637144576},

            //});
        }
    }
}
