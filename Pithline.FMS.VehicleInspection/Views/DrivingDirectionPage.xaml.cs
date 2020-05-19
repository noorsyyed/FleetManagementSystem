using Bing.Maps;
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Pithline.FMS.VehicleInspection.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrivingDirectionPage : VisualStateAwarePage, INotifyPropertyChanged
    {
        readonly Geolocator geolocator = new Geolocator();
        Pushpin pushpin;
        private Location location;     
        public Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        public DrivingDirectionPage()
        {
            this.InitializeComponent();
            Location = new Bing.Maps.Location();
            pushpin = new Pushpin { Visibility = Windows.UI.Xaml.Visibility.Collapsed };
            this.MyMap.Children.Add(pushpin);
            geolocator.PositionChanged += geolocator_PositionChanged;
        }

        async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                Location.Latitude = args.Position.Coordinate.Point.Position.Latitude;
                Location.Longitude = args.Position.Coordinate.Point.Position.Longitude;
                //Location = new Location(args.Position.Coordinate.Point.Position.Latitude, args.Position.Coordinate.Point.Position.Longitude);
                
                MapLayer.SetPosition(pushpin, location);
                pushpin.Visibility = Windows.UI.Xaml.Visibility.Visible;
                MyMap.SetView(location, 16);
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        private void pageRoot_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
