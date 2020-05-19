using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.DeliveryModel;
using Pithline.FMS.BusinessLogic.Enums;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.DocumentDelivery.UILogic.AifServices;
using Pithline.FMS.DocumentDelivery.UILogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pithline.FMS.DocumentDelivery.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        IEventAggregator _eventAggregator;
        public IdentityServiceProxy(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        async public Task<Tuple<CDLogonResult, string>> LogonAsync(string userId, string password)
        {
            try
            {
                await DDServiceProxyHelper.Instance.ConnectAsync(userId.Trim(), password.Trim(), _eventAggregator);
                var userInfo = await DDServiceProxyHelper.Instance.ValidateUser(userId.Trim(), password.Trim());
                if (userInfo == null)
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered password is incorrect, please verify the password you entered.");
                }
                else if (!String.IsNullOrWhiteSpace(userInfo.UserId))
                {
                    string jsonUserInfo = JsonConvert.SerializeObject(userInfo);
                    ApplicationData.Current.RoamingSettings.Values[Constants.UserInfo] = jsonUserInfo;
                    return new Tuple<CDLogonResult, string>(new CDLogonResult
                    {
                        UserInfo = userInfo

                    }, "");
                }
                else
                {
                    return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect,  please verify the password you entered");
                }
            }
            catch (Exception)
            {
                return new Tuple<CDLogonResult, string>(null, "Whoa! The entered username or password is incorrect, please verify the password you entered.");
            }
        }

        public Task<bool> VerifyAcitveSessionAsync(string userId)
        {
            throw new NotImplementedException();
        }

    }
}
