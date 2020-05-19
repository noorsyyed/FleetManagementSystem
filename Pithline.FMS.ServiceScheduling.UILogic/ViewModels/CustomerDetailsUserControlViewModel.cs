
using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Windows.System;
using System.Linq;
namespace Pithline.FMS.ServiceScheduling.UILogic.ViewModels
{
    public class CustomerDetailUserControlViewModel : ViewModel
    {
        string BingMapkey = "AkjXEdr_kqaXOMmwBdob0dSykNVdliqwxEufZJrzotABsNfEkkDFuiqSTbzPxZgs";
        public CustomerDetailUserControlViewModel()
        {
            this.Location = new Bing.Maps.Location();
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
                    //await this.GeocodeAddressAsync(Regex.Replace(address, "\n", ","));
                    //var stringBuilder = new StringBuilder("bingmaps:?rtp=pos.");
                    //stringBuilder.Append(Location.Latitude);
                    //stringBuilder.Append("_");
                    //stringBuilder.Append(Location.Longitude);
                       var stringBuilder = new StringBuilder("bingmaps:?where=" + Regex.Replace(address, "\n", ","));
                    await Launcher.LaunchUriAsync(new Uri(stringBuilder.ToString()));
                }, (address) =>
                {
                    return !string.IsNullOrEmpty(address);
                });
        }
        public DelegateCommand<string> MailToCommand { get; set; }

        public DelegateCommand<string> MakeIMCommand { get; set; }

        public DelegateCommand<string> LocateCommand { get; set; }

        public DelegateCommand<string> MakeSkypeCallCommand { get; set; }

        private Bing.Maps.Location location;
        public Bing.Maps.Location Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }

        //private async System.Threading.Tasks.Task GeocodeAddressAsync(string address)
        //{
        //    try
        //    {
        //        List<string> addressLines = new List<string>();
        //        if (address.Split(',').Count() > 3)
        //        {
        //            foreach (string line in address.Split(',').Reverse().Take(3))
        //            {
        //                addressLines.Add(line);
        //            }
        //            addressLines.Reverse();
        //            address = String.Join(",", addressLines);
        //        }
        //        GeocodeRequest geocodeRequest = new GeocodeRequest();

        //        geocodeRequest.Credentials = new GeocodeService.Credentials();
        //        geocodeRequest.Credentials.ApplicationId = BingMapkey;

        //        geocodeRequest.Query = address;

        //        GeocodeServiceClient geocodeService = new GeocodeServiceClient(Pithline.FMS.ServiceScheduling.UILogic.GeocodeService.GeocodeServiceClient.EndpointConfiguration.BasicHttpBinding_IGeocodeService);
        //        GeocodeResponse geocodeResponse = await geocodeService.GeocodeAsync(geocodeRequest);

        //        if (geocodeResponse.Results.Count > 0)
        //        {
        //            this.Location.Longitude = geocodeResponse.Results[0].Locations[0].Longitude;
        //            this.Location.Latitude = geocodeResponse.Results[0].Locations[0].Latitude;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppSettings.Instance.ErrorMessage = ex.Message;
        //    }
        //}
    }
}
