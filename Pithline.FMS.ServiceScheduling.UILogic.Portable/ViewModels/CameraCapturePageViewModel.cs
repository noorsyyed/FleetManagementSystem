using Pithline.FMS.BusinessLogic.Portable.SSModels;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Newtonsoft.Json;
using Pithline.FMS.BusinessLogic.Portable;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable
{

    public class CameraCapturePageViewModel : ViewModel
    {
        private INavigationService _navigationService;
        public ServiceSchedulingDetail _serviceDetail;
        private byte[] _bytes;

        public CameraCapturePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            TakePictureCommand = new DelegateCommand(async () =>
            {
                var imageEncodingProps = ImageEncodingProperties.CreatePng();

                using (var stream = new InMemoryRandomAccessStream())
                {
                    await this.MediaCapture.CapturePhotoToStreamAsync(imageEncodingProps, stream);
                    _bytes = new byte[stream.Size];
                    var buffer = await stream.ReadAsync(_bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
                    _bytes = buffer.ToArray(0, (int)stream.Size);

                    if (ImageSource == null)
                    {
                        ImageSource = new BitmapImage();
                    }
                    await this.ImageSource.SetSourceAsync(stream);
                    this.ImageVisibility = Visibility.Visible;
                }
            });

            RetakePictureCommand = new DelegateCommand(() =>
            {
                ImageVisibility = Visibility.Collapsed;
            });

            AcceptCommand = new DelegateCommand<byte[]>((bytes) =>
            {
                _serviceDetail.OdoReadingImageCapture.ImageBitmap = this.ImageSource;
                if (bytes.Length > 0)
                {
                    _serviceDetail.ODOReadingSnapshot = Convert.ToBase64String(bytes);
                }

                _navigationService.Navigate("ServiceScheduling", _serviceDetail);
                //_navigationService.ClearHistory();
            });

        }

    
        async public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            this.ImageVisibility = Visibility.Collapsed;
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.SSDCAMERACAPTURE))
            {
                _serviceDetail = JsonConvert.DeserializeObject<ServiceSchedulingDetail>(ApplicationData.Current.RoamingSettings.Values[Constants.SSDCAMERACAPTURE].ToString());
            }
      
            //await this.MediaCapture.InitializeAsync();
            //this.MediaCapture.VideoDeviceController.PrimaryUse = Windows.Media.Devices.CaptureUse.Photo;

            //await this.MediaCapture.StartPreviewAsync();

        }





        public ICommand TakePictureCommand { get; set; }

        public ICommand RetakePictureCommand { get; set; }

        public ICommand AcceptCommand { get; set; }

        private MediaCapture mediaCapture;
        public MediaCapture MediaCapture
        {
            get { return mediaCapture; }
            set { SetProperty(ref mediaCapture, value); }
        }

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        private Visibility imageVisibility;
        public Visibility ImageVisibility
        {
            get { return imageVisibility; }
            set { SetProperty(ref imageVisibility, value); }
        }
    }
}
