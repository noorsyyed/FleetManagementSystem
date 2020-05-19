using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Factories
{
    public class HttpFactory : IHttpFactory
    {
        ConnectionProfile _connectionProfile;
        async public Task<Windows.Web.Http.HttpResponseMessage> PostAsync(Windows.Web.Http.HttpStringContent data)
        {
            try
            {
                _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
                NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

                if (_connectionProfile != null && _connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess)
                {
                    using (var httpClient = new HttpClient())
                    {

                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("Content-Type: application/x-www-form-urlencoded"));
                        var token = JsonConvert.DeserializeObject<AccessToken>(ApplicationData.Current.RoamingSettings.Values[Constants.ACCESSTOKEN].ToString());
                        httpClient.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", token.Access_Token);

                        return await httpClient.PostAsync(new Uri(Constants.APIURL), data);
                    }
                }
                else
                {
                    await new MessageDialog("no network access").ShowAsync();
                    return default(HttpResponseMessage);
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
                return default(HttpResponseMessage);
            }
        }

        void NetworkInformation_NetworkStatusChanged(object sender)
        {
            try
            {
                _connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }
        }
    }
}
