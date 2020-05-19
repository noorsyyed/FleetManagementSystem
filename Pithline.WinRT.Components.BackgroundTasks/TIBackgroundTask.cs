
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Pithline.FMS.BusinessLogic.Common;
using Windows.Data.Xml.Dom;
using Pithline.FMS.TechnicalInspection.UILogic.AifServices;
using Pithline.FMS.TechnicalInspection.UILogic.Services;
using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.PubSubEvents;
namespace Pithline.FMS.WinRT.Components.BackgroundTasks
{

    public sealed class TIBackgroundTask : IBackgroundTask
    {
        private ICredentialStore _credentialStore = new RoamingCredentialStore();
        private string PasswordVaultResourceName = "TechnicalInspection";
        private string textElementName = "text";
        async public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            SqliteHelper.Storage.ConnectionDatabaseAsync();
            await GetTasksforLiveTile();
            deferral.Complete();
        }

        async private System.Threading.Tasks.Task GetTasksforLiveTile()
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
                TIServiceHelper.Instance.ConnectAsync(crd.UserName, crd.Password, new EventAggregator());
                var allTasks = await TIServiceHelper.Instance.SyncTasksFromAXAsync();

                if (allTasks != null)
                {
                    foreach (var collection in allTasks.InSetsOf(3))
                    {
                        int index = 0;

                        XmlDocument tileXmlSquare = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare310x310TextList01);

                        var bindingElementSquare = (XmlElement)tileXmlSquare.GetElementsByTagName("binding").Item(0);
                        bindingElementSquare.SetAttribute("branding", "name");

                        XmlNodeList tileTextAttributes = tileXmlSquare.GetElementsByTagName("text");

                        foreach (var item in collection)
                        {
                            XmlDocument tileXmlWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideBlockAndText02);

                            var bindingElemtWide = (XmlElement)tileXmlWide.GetElementsByTagName("binding").Item(0);
                            bindingElemtWide.SetAttribute("branding", "name");

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
