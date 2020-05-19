using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.System;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
   public class CustomerDetailUserControlViewModel : ViewModel
    {
       public CustomerDetailUserControlViewModel()
       {
           this.MakeIMCommand = DelegateCommand<string>.FromAsyncHandler(async(emailId)=>
               {
                   await Launcher.LaunchUriAsync(new Uri("skype:shoaibrafi?chat"));
               }, (emailId) => { return !string.IsNullOrEmpty(emailId); });

           this.MakeSkypeCallCommand = DelegateCommand<string>.FromAsyncHandler(async (number) =>
               {
                   await Launcher.LaunchUriAsync(new Uri("audiocall-skype-com:" + number));
               }, (number) => { return !string.IsNullOrEmpty(number); });

           this.MailToCommand = DelegateCommand<string>.FromAsyncHandler(async (email) =>
               {
                   await Launcher.LaunchUriAsync(new Uri("mailto:"+email));
               }, (email) => { return !string.IsNullOrEmpty(email); });

           this.LocateCommand = DelegateCommand<string>.FromAsyncHandler(async (address) =>
               {
                   await Launcher.LaunchUriAsync(new Uri("bingmaps:?where="+ Regex.Replace(address, "\n", ",")));
               }, (address) =>
               {
                   return !string.IsNullOrEmpty(address);
               });
       }
       public DelegateCommand<string> MailToCommand { get; set; }

       public DelegateCommand<string> MakeIMCommand { get; set; }

       public DelegateCommand<string> LocateCommand { get; set; }

       public DelegateCommand<string> MakeSkypeCallCommand { get; set; }

       
    }
}
