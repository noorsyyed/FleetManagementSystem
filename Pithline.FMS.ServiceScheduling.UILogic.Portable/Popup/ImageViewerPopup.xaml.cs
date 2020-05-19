using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Portable;
using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pithline.FMS.ServiceScheduling
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageViewerPopup : ContentDialog
    {
        private TranslateTransform ct;
        private bool isImageDeleted;
        private double originalY;
        private double originalX;
        private Image img;
        private ServiceSchedulingDetail _model;
        private IEventAggregator _eventAggregator;

        public ImageViewerPopup(IEventAggregator eventAggregator, ServiceSchedulingDetail model)
        {
            this.InitializeComponent();
            this._eventAggregator = eventAggregator;
        }
        private void Image_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var datacontext = this.DataContext as Pithline.FMS.BusinessLogic.Portable.ImageCapture;

            img = sender as Image;
            ct = img.RenderTransform as TranslateTransform;

            if (ct == null) return;

            ct.Y += e.Delta.Translation.Y;

            if (ct.Y > 400)
            {
                datacontext = new Pithline.FMS.BusinessLogic.Portable.ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png", ImageBitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/ODO_meter.png")) }; ;
                isImageDeleted = true;
                _model.ODOReadingSnapshot = string.Empty;
                this.Hide();
                this._eventAggregator.GetEvent<ImageCaptureEvent>().Publish(datacontext);
            }
            else if (ct.Y < -400)
            {
                datacontext = new Pithline.FMS.BusinessLogic.Portable.ImageCapture { ImagePath = "ms-appx:///Assets/ODO_meter.png", ImageBitmap = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/ODO_meter.png")) }; ;
                isImageDeleted = true;
                _model.ODOReadingSnapshot = string.Empty;
                this.Hide();
                this._eventAggregator.GetEvent<ServiceSchedulingDetailEvent>().Publish(_model);
            }

        }

        private void Image_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!isImageDeleted)
            {
                ct.X = originalX;
                ct.Y = originalY;
                e.Handled = true;
            }
        }

        private void Image_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            Image img = sender as Image;
            ct = img.RenderTransform as TranslateTransform;
            originalX = ct.X;
            originalY = ct.Y;

        }
    }
}
