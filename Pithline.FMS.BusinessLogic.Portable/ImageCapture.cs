using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;


namespace Pithline.FMS.BusinessLogic.Portable
{
    public class ImageCaptureEvent : PubSubEvent<ImageCapture>
    {

    }
    public class ImageCapture : BindableBase
    {
        public int Id { get; set; }

        public string ImagePath { get; set; }

        private string imageBinary;

        public string ImageBinary { get; set; }

        public string FileName { get; set; }

        public long CaseServiceRecId { get; set; }

        public long PrimeId { get; set; }

        public bool IsSynced { get; set; }

        private BitmapImage imageBitmap;
        public BitmapImage ImageBitmap
        {
            get { return imageBitmap; }
            set { SetProperty(ref imageBitmap, value); }
        }
    }
}
