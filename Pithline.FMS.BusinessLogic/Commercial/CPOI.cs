using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.BusinessLogic.Commercial
{
    public class CPOI : BaseModel
    {
     
            public async override Task<BaseModel> GetDataAsync(long vehicleInsRecID)
            {
                return await SqliteHelper.Storage.GetSingleRecordAsync<CPOI>(x => x.VehicleInsRecID == vehicleInsRecID);
            }
            public CPOI()
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
                set { SetProperty(ref cRDate, value); }
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

            private BitmapImage custSignature;
                [Ignore]
            public BitmapImage CustSignature
            {
                get { return custSignature; }
                set
                {
                    if (SetProperty(ref custSignature, value))
                    {
                        CRDate = DateTime.Now;
                    }

                }
            }

            private BitmapImage pithlineRepSignature;
                    [Ignore]
            public BitmapImage PithlineRepSignature
            {
                get { return PithlineRepSignature; }
                set
                {
                    if (SetProperty(ref pithlineRepSignature, value))
                    {
                        EQRDate = DateTime.Now;
                    }
                }
            }
            

            private bool isSellChecked;

            public bool IsSellChecked
            {
                get { return isSellChecked; }
                set { SetProperty(ref isSellChecked, value); }
            }


            private bool isNotFeasChecked;

            public bool IsNotFeasChecked
            {
                get { return isNotFeasChecked; }
                set { SetProperty(ref isNotFeasChecked, value); }
            }

            private bool isRetainChecked;

            public bool IsRetainChecked
            {
                get { return isRetainChecked; }
                set { SetProperty(ref isRetainChecked, value); }
            }


            private bool isGoodChecked;

            public bool IsGoodChecked
            {
                get { return isGoodChecked; }
                set { SetProperty(ref isGoodChecked, value); }
            }
            private bool isFairChecked;

            public bool IsFairChecked
            {
                get { return isFairChecked; }
                set { SetProperty(ref isFairChecked, value); }
            }

            private bool isPoorChecked;

            public bool IsPoorChecked
            {
                get { return isPoorChecked; }
                set { SetProperty(ref isPoorChecked, value); }
            }
        
        }
    }

