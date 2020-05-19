using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Common;
using Pithline.FMS.BusinessLogic.Helpers;
using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.VehicleInspection.UILogic.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pithline.FMS.VehicleInspection.UILogic.ViewModels
{
    public class InspectionProofUserControlViewModel : BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        public InspectionProofUserControlViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Model = new PInspectionProof();
            _eventAggregator = eventAggregator;
        }

        public async override System.Threading.Tasks.Task LoadModelFromDbAsync(long vehicleInsRecID)
        {
            this.Model = await SqliteHelper.Storage.GetSingleRecordAsync<PInspectionProof>(x => x.VehicleInsRecID == vehicleInsRecID);
            if (this.Model == null)
            {
                this.Model = new PInspectionProof();
            }
            BaseModel viBaseObject = (PInspectionProof)this.Model;
            viBaseObject.VehicleInsRecID = vehicleInsRecID;
            viBaseObject.LoadSnapshotsFromDb();

            viBaseObject.ShouldSave = false;
            PropertyHistory.Instance.SetPropertyHistory(viBaseObject);
       
        }

        private BitmapImage custSignature;
        
        public BitmapImage CustSignature
        {
            get { return custSignature; }
            set
            {
                if (SetProperty(ref custSignature, value))
                {
                    ((PInspectionProof)this.Model).CRDate = DateTime.Now;
                    _eventAggregator.GetEvent<SignChangedEvent>().Publish(true);
                }

            }
        }

        private BitmapImage pithlineRepSignature;
        
        public BitmapImage PithlineRepSignature
        {
            get { return  pithlineRepSignature; }
            set
            {
                if (SetProperty(ref pithlineRepSignature, value))
                {
                   ((PInspectionProof)this.Model).EQRDate = DateTime.Now;
                   _eventAggregator.GetEvent<SignChangedEvent>().Publish(true);
                }
            }
        }


    }
}
