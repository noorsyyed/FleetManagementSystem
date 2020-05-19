using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Factories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.Web.Http;

namespace Pithline.FMS.TechnicalInspection.UILogic.WindowsPhone.Services
{
    public class UserService : IUserService
    {
        IHttpFactory _httpFactory;
        public UserService(IHttpFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        async public Task<UserInfo> GetUserInfoAsync(string userId)
        {
            try
            {
                var postData = new { target = "TechnicalInspection", method = "single", parameters = new[] { "GetUserInfo", userId } };
                var response = await _httpFactory.PostAsync(new HttpStringContent(JsonConvert.SerializeObject(postData), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }

            return null;
        }




        async public Task<AccessToken> ValidateUserAsync(string userName, string password)
        {
            var postData = "grant_type=password&username=" + userName + "&password=" + password;

            try
            {
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Accept.Add(new Windows.Web.Http.Headers.HttpMediaTypeWithQualityHeaderValue("Content-Type: application/x-www-form-urlencoded"));
                    var response = await httpClient.PostAsync(new Uri(Constants.TOKENURL), new HttpStringContent(postData, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var accessToken = JsonConvert.DeserializeObject<AccessToken>(await response.Content.ReadAsStringAsync());
                        accessToken.ExpirationDate = DateTime.Now.AddSeconds(accessToken.Expires_In);
                        return accessToken;
                    }
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();

                return null ;
            }


            return null;
        }
    }
}
