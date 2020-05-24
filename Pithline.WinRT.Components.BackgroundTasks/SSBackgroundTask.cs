using Pithline.FMS.ServiceScheduling.UILogic.AifServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Pithline.FMS.BusinessLogic.Common;
using Microsoft.Practices.Unity;
using Pithline.FMS.ServiceScheduling.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace Pithline.WinRT.Components.BackgroundTasks
{
    public sealed class SSBackgroundTask : IBackgroundTask
    {
        private ICredentialStore _credentialStore = new RoamingCredentialStore();
        private string PasswordVaultResourceName = "ServiceScheduling";
        private string textElementName = "text";
        async public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            await GetTasksforLiveTile();
            deferral.Complete();
        }

        async private Task GetTasksforLiveTile()
        {
            try
            {

                var updaterWide = TileUpdateManager.CreateTileUpdaterForApplication();
                var updaterSqure = TileUpdateManager.CreateTileUpdaterForApplication();
                var updaterBadge = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
                updaterWide.EnableNotificationQueue(true);
                updaterSqure.EnableNotificationQueue(true);
                updaterWide.Clear();
                updaterSqure.Clear();
                updaterBadge.Clear();
                int counter = 0;

                var crd = _credentialStore.GetSavedCredentials(PasswordVaultResourceName);

                SSProxyHelper.Instance.ConnectAsync(crd.UserName, crd.Password);
                var allTasks = (SSProxyHelper.Instance.GetTasksFromSvcAsync()).Result;

                if (allTasks != null)
                {
                    foreach (var collection in allTasks.InSetsOf(3))
                    {
                        int index = 0;

                        XmlDocument tileXmlSquare = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310TextList01);

                        var bindingElement = (XmlElement)tileXmlSquare.GetElementsByTagName("binding").Item(0);
                        bindingElement.SetAttribute("branding", "name");

                        XmlNodeList tileTextAttributes = tileXmlSquare.GetElementsByTagName("text");

                        foreach (var item in collection)
                        {
                            XmlDocument tileXmlWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideBlockAndText02);

                            var bindingElemt = (XmlElement)tileXmlWide.GetElementsByTagName("binding").Item(0);
                            bindingElemt.SetAttribute("branding", "name");

                            tileTextAttributes[index].InnerText = item.CustomerName.ToString();
                            tileTextAttributes[++index].InnerText = item.ContactName.ToString();
                            tileTextAttributes[++index].InnerText = item.StatusDueDate.Day.ToString() + " " + item.StatusDueDate.ToString("MMMM");

                            tileXmlWide.GetElementsByTagName(textElementName)[0].InnerText = item.CustomerName;
                            tileXmlWide.GetElementsByTagName(textElementName)[1].InnerText = item.StatusDueDate.Day.ToString();
                            tileXmlWide.GetElementsByTagName(textElementName)[2].InnerText = item.StatusDueDate.ToString("MMMM");

                            updaterWide.Update(new TileNotification(tileXmlWide));
                            ++index;
                        }
                        if (counter++ > 6)
                        {
                            break;
                        }
                        updaterSqure.Update(new TileNotification(tileXmlSquare));
                    }


                    XmlDocument BadgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                    var badgeAttributes = BadgeXml.GetElementsByTagName("badge");
                    ((XmlElement)badgeAttributes[0]).SetAttribute("value", allTasks.Count.ToString());

                    updaterBadge.Update(new BadgeNotification(BadgeXml));
                }
            }
            catch (Exception)
            {

            }
        }
    }
}


