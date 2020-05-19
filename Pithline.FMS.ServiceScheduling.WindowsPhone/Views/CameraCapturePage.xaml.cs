using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Pithline.FMS.ServiceScheduling.UILogic.Portable;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.ServiceScheduling.WindowsPhone.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraCapturePage : VisualStateAwarePage
    {
        MediaCapture _mediaCapture;
        byte[] _bytes;
        public CameraCapturePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        async private void PreviewElement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _mediaCapture = new MediaCapture();
                _mediaCapture.Failed += mediaCapture_Failed;

                var _deviceInformation = await GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel.Back);

                var settings = new MediaCaptureInitializationSettings();
                //settings.StreamingCaptureMode = StreamingCaptureMode.Video;
                settings.PhotoCaptureSource = PhotoCaptureSource.Photo;
                settings.AudioDeviceId = "";
                if (_deviceInformation != null)
                    settings.VideoDeviceId = _deviceInformation.Id;

                await _mediaCapture.InitializeAsync(settings);

                var focusSettings = new FocusSettings();
                focusSettings.AutoFocusRange = AutoFocusRange.FullRange;
                focusSettings.Mode = FocusMode.Auto;
                focusSettings.WaitForFocus = true;
                focusSettings.DisableDriverFallback = false;

                _mediaCapture.VideoDeviceController.FocusControl.Configure(focusSettings);
                await _mediaCapture.VideoDeviceController.ExposureControl.SetAutoAsync(true);

                //_mediaCapture.SetPreviewRotation(VideoRotation.Clockwise90Degrees);
                //_mediaCapture.SetRecordRotation(VideoRotation.Clockwise90Degrees);


                PreviewElement.Source = _mediaCapture;
                await _mediaCapture.StartPreviewAsync();
                //mediaCapture.VideoDeviceController.PrimaryUse = Windows.Media.Devices.CaptureUse.Photo;
                //PreviewElement.Source = mediaCapture;
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message, "Error");
            }
        }

        private void mediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            throw new NotImplementedException();
        }

        private static async Task<DeviceInformation> GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {

            DeviceInformation device = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(d => d.EnclosureLocation != null && d.EnclosureLocation.Panel == desiredPanel);

            if (device == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable devices found for the camera of type {0}.", desiredPanel));
            }
            return device;
        }

        async private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vm = this.DataContext as CameraCapturePageViewModel;
                var imageEncodingProps = ImageEncodingProperties.CreatePng();
                using (var stream = new InMemoryRandomAccessStream())
                {

                    await _mediaCapture.CapturePhotoToStreamAsync(imageEncodingProps, stream);
                    _bytes = new byte[stream.Size];
                    var buffer = await stream.ReadAsync(_bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
                    _bytes = buffer.ToArray(0,(int)stream.Size);

                    if (vm.ImageSource == null)
                    {
                        vm.ImageSource = new BitmapImage();
                    }
                    stream.Seek(0);
                    await vm.ImageSource.SetSourceAsync(stream);
                    Retake.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    Take.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    await _mediaCapture.StopPreviewAsync();

                   
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       async private void Accept_Click(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as CameraCapturePageViewModel;
            if (Take.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                await _mediaCapture.StopPreviewAsync(); 
            }
            _mediaCapture.Dispose();
            vm.AcceptCommand.Execute(_bytes);
        }

       async private void Retake_Click(object sender, RoutedEventArgs e)
        {
            this.Retake.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.Take.Visibility = Windows.UI.Xaml.Visibility.Visible;
            await _mediaCapture.StartPreviewAsync();
        }


    }
}
