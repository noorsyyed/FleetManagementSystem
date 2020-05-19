using Pithline.FMS.DataProvider.AX.Repositories;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Pithline.FMS.CrossPlatform.API
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
       async public override System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                context.Validated();
            }
            catch (Exception)
            {

                throw;
            }
        }
        async public override System.Threading.Tasks.Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                var header = context.OwinContext.Response.Headers.SingleOrDefault(h => h.Key == "Access-Control-Allow-Origin");
                if (header.Equals(default(KeyValuePair<string, string[]>)))
                {
                    context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                }
                using (SSRepository ssRepo = new SSRepository())
                {
                    if (await ssRepo.ValidateUserAsync(null, context.UserName, context.Password))
                    {
                        context.SetError("invalid_grant", "The username or password is incorrect.");
                        return;
                    }


                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("role", "user"));
                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("serviceunreachable", ex.Message.ToString());
                return;
            }
            
        }
    }
}
