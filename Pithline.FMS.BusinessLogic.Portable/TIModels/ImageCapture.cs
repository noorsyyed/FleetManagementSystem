using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.BusinessLogic.Portable.TIModels
{
    public class ImageCapture : BindableBase
    {
        public long VehicleInspecId { get; set; }

        public string CaseNumber { get; set; }

        public long RepairId { get; set; }

        public string Component { get; set; }

        public string ImagePath { get; set; }

        public string FileName { get; set; }

        public string ImageData { get; set; }

        private BitmapImage imageBitmap;
        public BitmapImage ImageBitmap
        {
            get { return imageBitmap; }
            set { SetProperty(ref imageBitmap, value); }
        }
        public Guid guid { get; set; }

        private bool isMajorPivot;
        public bool IsMajorPivot
        {
            get { return isMajorPivot; }
            set { SetProperty(ref isMajorPivot, value); }
        }
    }

    public class ImageCaptureTranEvent : PubSubEvent<Pithline.FMS.BusinessLogic.Portable.TIModels.ImageCapture>
    {

    }
}

