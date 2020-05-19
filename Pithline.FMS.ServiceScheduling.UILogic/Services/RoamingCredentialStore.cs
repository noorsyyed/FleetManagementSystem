using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Pithline.FMS.ServiceScheduling.UILogic.Services
{
    public class RoamingCredentialStore : ICredentialStore
    {
        public void SaveCredentials(string resource, string userName, string password)
        {
            var vault = new PasswordVault();

            RemoveAllCredentialsByResource(resource, vault);

            // Add the new credential 
            var passwordCredential = new PasswordCredential(resource, userName, password);
            vault.Add(passwordCredential);
        }

        private static void RemoveAllCredentialsByResource(string resource, PasswordVault vault)
        {
            try
            {
                // Remove the old credentials for this resource
                var oldCredentials = vault.FindAllByResource(resource);
                foreach (var oldCredential in oldCredentials)
                {
                    vault.Remove(oldCredential);
                }
            }
            catch (Exception)
            {
            } // FindAllByResource throws Exception if nothing stored for that resource
        }

        public PasswordCredential GetSavedCredentials(string resource)
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

        public void RemoveSavedCredentials(string resource)
        {
            var vault = new PasswordVault();
            RemoveAllCredentialsByResource(resource, vault);
        }
    }
}
