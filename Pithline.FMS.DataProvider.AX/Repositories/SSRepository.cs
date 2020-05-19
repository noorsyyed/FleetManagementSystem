using Pithline.FMS.DataProvider.AX.SSProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.Repositories
{
    public class SSRepository : IDisposable
    {

        async public Task<bool> ValidateUserAsync(MzkServiceSchedulingServiceClient client, string userId, string password)
        {
            UserInfo userInfo = new UserInfo();
            try
            {
                if (client == null)
                {
                    client = GetServiceClient();
                }
                return !(await client.validateUserAsync(new CallContext() { }, userId, password)).response;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public MzkServiceSchedulingServiceClient GetServiceClient()
        {
            try
            {
                MzkServiceSchedulingServiceClient client;
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
                basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                client = new MzkServiceSchedulingServiceClient(basicHttpBinding, new EndpointAddress("http://srfmlbispstg01.lfmd.co.za/MicrosoftDynamicsAXAif60/SSService/xppservice.svc"));
                client.ClientCredentials.UserName.UserName = "lfmd" + "\"" + "axbcsvc";
                client.ClientCredentials.UserName.Password = "AXrocks100";
                client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                client.ClientCredentials.Windows.ClientCredential = new NetworkCredential("axbcsvc", "AXrocks100", "lfmd");
                return client;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void Dispose()
        {

        }
    }
}
