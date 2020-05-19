using Pithline.FMS.BusinessLogic.Portable.TIModels;
using Pithline.FMS.WinRT.Components.Controls.WindowsPhone;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Pithline.FMS.TechnicalInspection.UILogic
{
    public sealed partial class CameraCaptureDialog : ContentDialog
    {
        MediaCapture _mediaCapture;
        byte[] _bytes;
        public CameraCaptureDialog()
        {
            this.InitializeComponent();
            this.Loaded += CameraCaptureDialog_Loaded;
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.Closing += CameraCaptureDialog_Closing;

        }

        async void CameraCaptureDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {

            await _mediaCapture.StopPreviewAsync();
        }

        async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
        }

        async void CameraCaptureDialog_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private static async Task<DeviceInformation> GetCameraDeviceInfoAsync(Windows.Devices.Enumeration.Panel desiredPanel)
        {

            try
            {
                DeviceInformation device = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                       .FirstOrDefault(d => d.EnclosureLocation != null && d.EnclosureLocation.Panel == desiredPanel);

                if (device == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No suitable devices found for the camera of type {0}.", desiredPanel));
                }
                return device;
            }
            catch (Exception)
            {

                return null;
            }
        }

        async private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Img.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                args.Cancel = false;

            }
            else
            {
                args.Cancel = true;
            }
            this.Hide();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                args.Cancel = true;
                Img.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                PreviewElement.Visibility = Windows.UI.Xaml.Visibility.Visible;

            }
            catch (Exception)
            {

            }
        }

        async private void PreviewElement_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                _mediaCapture = new MediaCapture();


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
            }
            catch (Exception)
            {

            }
        }

        private async static System.Threading.Tasks.Task ByteArrayToBitmapImage(byte[] imageBytes)
        {
            try
            {
                if (imageBytes.Any())
                {
                    var filename ="test" + ".png";

                    var sampleFile = await KnownFolders.PicturesLibrary.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteBytesAsync(sampleFile, imageBytes);
                }

            }
            catch (Exception)
            {
            }
        }



        async private void PreviewElement_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var bi = new BusyIndicator();
            try
            {
                bi.Open("Please wait");
                var imageEncodingProps = ImageEncodingProperties.CreatePng();
                using (var stream = new InMemoryRandomAccessStream())
                {

                    await _mediaCapture.CapturePhotoToStreamAsync(imageEncodingProps, stream);
                    _bytes = new byte[stream.Size];
                    var buffer = await stream.ReadAsync(_bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
                    _bytes = buffer.ToArray(0, (int)stream.Size);
                    await ByteArrayToBitmapImage(_bytes);
                    var bitmap = new BitmapImage();
                    stream.Seek(0);
                    await bitmap.SetSourceAsync(stream);
                    var model = this.Tag as MaintenanceRepair;
                    if (model == null)
                    {
                        model = new MaintenanceRepair();
                    }
                    if (model.IsMajorPivot)
                    {
                        model.MajorComponentImgList.Add(new ImageCapture
                               {
                                   ImageBitmap = bitmap
                               });
                    }
                    else
                    {
                        model.SubComponentImgList.Add(new ImageCapture
                        {
                            ImageBitmap = bitmap
                        });
                    }

                    PreviewElement.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Img.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    this.IsSecondaryButtonEnabled = true;
                }
                bi.Close();
            }
            catch (Exception)
            {
                bi.Close();

            }
        }


    }
}
