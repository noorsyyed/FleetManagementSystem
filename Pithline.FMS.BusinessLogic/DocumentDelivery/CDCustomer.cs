

using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.DocumentDelivery
{
    public class CDCustomer : DocumentsReceiver
    {
        private bool isprimary;

        public bool Isprimary
        {
            get { return isprimary; }
            set { SetProperty(ref isprimary, value); }
        }

    }
}
