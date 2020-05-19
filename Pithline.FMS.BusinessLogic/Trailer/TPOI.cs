using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.BusinessLogic
{
    public class TPOI : BaseModel
    {
        public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
        {
            return await SqliteHelper.Storage.GetSingleRecordAsync<TPOI>(x => x.VehicleInsRecID == vehicleInsRecID);
        }
        public TPOI()
        {
            this.CRTime = DateTime.Now;
            this.CRDate = DateTime.Now;
            this.EQRDate = DateTime.Now;
            this.EQRTime = DateTime.Now;
            this.CRSignFileName = "cr_" + new Random().Next(1000) + TimeSpan.TicksPerMillisecond;
            this.EQRSignFileName = "eqr_" + new Random().Next(1000) + TimeSpan.TicksPerMillisecond;
        }

        private string crSignFileName;

        public string CRSignFileName
        {
            get { return crSignFileName; }
            set { SetProperty(ref crSignFileName, value); }
        }


        private string eqrSignFileName;

        public string EQRSignFileName
        {
            get { return eqrSignFileName; }
            set { SetProperty(ref eqrSignFileName, value); }
        }

        private DateTime cRDate;
        public DateTime CRDate
        {
            get { return cRDate; }
            set
            { SetProperty(ref cRDate, value); }
        }

        private DateTime cRTime;
        public DateTime CRTime
        {
            get { return cRTime; }
            set { SetProperty(ref cRTime, value); }
        }

        private DateTime eQRDate;
        public DateTime EQRDate
        {
            get { return eQRDate; }
            set { SetProperty(ref eQRDate, value); }
        }

        private DateTime eQRTime;
        public DateTime EQRTime
        {
            get { return eQRTime; }
            set { SetProperty(ref eQRTime, value); }
        }
        private string cRSignComment;

        public string CRSignComment
        {
            get { return cRSignComment; }
            set { SetProperty(ref cRSignComment, value); }
        }

        private string eQRSignComment;
        public string EQRSignComment
        {
            get { return eQRSignComment; }
            set { SetProperty(ref eQRSignComment, value); }
        }

    }
}
