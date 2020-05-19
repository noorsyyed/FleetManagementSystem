using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
namespace Pithline.FMS.BusinessLogic.Portable
{
    public static class Util
    {

        async public static System.Threading.Tasks.Task WriteToDiskAsync<T>(T obj, string fileName)
        {
            string content = JsonConvert.SerializeObject(obj);
            StorageFile itemsSourceFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(itemsSourceFile, content);

        }
        public static BitmapImage FromBase64(string base64)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64);
                var imageStream = bytes.AsBuffer().AsStream().AsRandomAccessStream();
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(imageStream);
                return bitmapImage;
            }
            catch (Exception)
            {
                return default(BitmapImage);
            }
        }
        async public static System.Threading.Tasks.Task<T> ReadFromDiskAsync<T>(string fileName)
        {
            try
            {
                string content = string.Empty;
                var itemsSourceFile = await ApplicationData.Current.TemporaryFolder.GetItemAsync(fileName) as StorageFile;
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
