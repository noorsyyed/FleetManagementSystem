
using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.TechnicalInspection.UILogic.AifServices;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pithline.FMS.TechnicalInspection.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        IEventAggregator _eventAggregator;
        public IdentityServiceProxy(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        async public Task<Tuple<LogonResult, string>> LogonAsync(string userId, string password)
        {
            try
            {
                TIServiceHelper.Instance.ConnectAsync(userId.Trim(), password.Trim(), _eventAggregator);
                var result = await TIServiceHelper.Instance.ValidateUser(userId.Trim(), password.Trim());

                if (result.response)
                {
                    var ui = await TIServiceHelper.Instance.GetUserInfoAsync(userId.Trim());
                    if (ui.response != null)
                    {
                        var userInfo = new UserInfo
                                        {
                                            UserId = ui.response.parmUserID,
                                            CompanyId = ui.response.parmCompany,
                                            CompanyName = ui.response.parmCompanyName,
                                            Name = ui.response.parmUserName
                                        };
                        string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                        ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                        return new Tuple<LogonResult, string>(new LogonResult
                        {
                            UserInfo = userInfo

                        }, "");
                    }
                    return new Tuple<LogonResult, string>(new LogonResult
                    {
                        UserInfo = new UserInfo()

                    }, ""); ;
                }
                else
                {
                    return new Tuple<LogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify and try again.");
                }
            }
            catch (Exception)
            {
                return new Tuple<LogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify and try again.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
