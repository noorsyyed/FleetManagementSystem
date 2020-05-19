using Bing.Maps;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Geolocation;
using Windows.System;
using Windows.UI.Core;

namespace Pithline.FMS.DocumentDelivery.UILogic.ViewModels
{
    public class CustomerDetailUserControlViewModel : ViewModel
    {
        readonly Geolocator geolocator = new Geolocator();
        public CustomerDetailUserControlViewModel()
        {
            Location = new Bing.Maps.Location();

            geolocator.PositionChanged += geolocator_PositionChanged;

            this.MakeIMCommand = DelegateCommand<string>.FromAsyncHandler(async (emailId) =>
                {
                    await Launcher.LaunchUriAsync(new Uri("skype:shoaibrafi?chat"));
                }, (emailId) => { return !string.IsNullOrEmpty(emailId); });

            this.MakeSkypeCallCommand = DelegateCommand<string>.FromAsyncHandler(async (number) =>
                {
                    await Launcher.LaunchUriAsync(new Uri("audiocall-skype-com:" + number));
                }, (number) => { return !string.IsNullOrEmpty(number); });

            this.MailToCommand = DelegateCommand<string>.FromAsyncHandler(async (email) =>
                {
                    await Launcher.LaunchUriAsync(new Uri("mailto:" + email));
                }, (email) => { return !string.IsNullOrEmpty(email); });

            this.LocateCommand = DelegateCommand<string>.FromAsyncHandler(async (address) =>
                {
                    await Launcher.LaunchUriAsync(new Uri("bingmaps:?where=" + Regex.Replace(address, "\n", ",")));
                }, (address) =>
                {
                    return !string.IsNullOrEmpty(address);
                });

            GetDirectionsCommand = DelegateCommand<string>.FromAsyncHandler(async (address) =>
            {
                var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                stringBuilder.Append(Location.Latitude);
                stringBuilder.Append("_");
                stringBuilder.Append(Location.Longitude);
                stringBuilder.Append("~adr." + Regex.Replace(address, "\n", ","));
                await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
            });
        }

        async private void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Location.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                Location.Longitude = args.Position.Coordinate.Point.Position.Longitude;
            });

        }
        private Location location;
        public Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        public ICommand GetDirectionsCommand { get; set; }
        public DelegateCommand<string> MailToCommand { get; set; }

        public DelegateCommand<string> MakeIMCommand { get; set; }

        public DelegateCommand<string> LocateCommand { get; set; }

        public DelegateCommand<string> MakeSkypeCallCommand { get; set; }

    }
}
