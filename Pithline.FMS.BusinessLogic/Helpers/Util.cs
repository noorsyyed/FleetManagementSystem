using Pithline.FMS.BusinessLogic.ServiceSchedule;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Notifications;

namespace Pithline.FMS.BusinessLogic.Helpers
{
    public static class Util
    {
        public static void ShowToast(string message)
        {
            var toastXmlString = string.Format("<toast><visual version='1'><binding template='ToastText01'><text id='1'>{0}</text></binding></visual></toast>", message);
            var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDoc.LoadXml(toastXmlString);
            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        async public static System.Threading.Tasks.Task WriteToDiskAsync(string content, string fileName)
        {
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);

        }

        async public static System.Threading.Tasks.Task<List<T>> ReadFromDiskAsync<T>(string fileName)
        {
            try
            {
                string content = string.Empty;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    content = await FileIO.ReadTextAsync(itemsSourceFile);
                }
                return JsonConvert.DeserializeObject<List<T>>(content);

            }
            catch (Exception)
            {
                return null;
            }
        }

        async public static Task<T> DeserializeObjectAsync<T>(string fileName)
        {
            try
            {
                string content = string.Empty;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.TryGetItemAsync(fileName) as StorageFile;
                if (itemsSourceFile != null)
                {
                    content = await FileIO.ReadTextAsync(itemsSourceFile);
                }
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception)
            {
                return default(T);
            }

        }
    }
}
