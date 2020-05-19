using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Pithline.FMS.BusinessLogic.Common;
using Windows.UI.Popups;
using Pithline.FMS.BusinessLogic.Passenger;
using Pithline.FMS.BusinessLogic.Commercial;

namespace Pithline.FMS.BusinessLogic.Base
{
    abstract public class BaseModel : ValidatableBindableBase
    {
        private string caseNumber;        
        public string CaseNumber
        {
            get { return caseNumber; }
            set { caseNumber = value; }
        }

        private ObservableCollection<ValidationError> _errors = new ObservableCollection<ValidationError>();

        private long vehicleInsRecID;
        [PrimaryKey]
        public long VehicleInsRecID
        {
            get { return vehicleInsRecID; }
            set { SetProperty(ref vehicleInsRecID, value); }
        }


        private int tableId;

        public int TableId
        {
            get { return tableId; }
            set { SetProperty(ref tableId, value); }
        }

        private long recID;

        public long RecID
        {
            get { return recID; }
            set { SetProperty(ref recID, value); }
        }
        public ObservableCollection<ValidationError> Errors
        {
            get { return _errors; }
            private set { SetProperty(ref _errors, value); }
        }

        private bool shouldSave;

        public bool ShouldSave
        {
            get { return shouldSave; }
            set
            {
                shouldSave = value;
                OnPropertyChanged("ShouldSave");
            }
        }

        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            bool isOriginalchanged = false;
            bool result = base.SetProperty<T>(ref storage, value, propertyName);

            if (PropertyHistory.Instance.StorageHistory.Any())
            {
                isOriginalchanged = PropertyHistory.Instance.IsPropertyOriginalValueChanged(this);
            }
            return ShouldSave =isOriginalchanged;
        }

        public abstract System.Threading.Tasks.Task<BaseModel> GetDataAsync(long vehicleInsRecID);


        public bool ValidateModel()
        {
            _errors.Clear();
            var propertiesToValidate = this.GetType().GetRuntimeProperties().Where(o => o.GetCustomAttributes(typeof(DamageSnapshotRequiredAttribute)).Any());
            foreach (var propInfo in propertiesToValidate)
            {
                var propValue = propInfo.GetValue(this);
                var att = propInfo.GetCustomAttribute<DamageSnapshotRequiredAttribute>();
                var associatedPropInfo = this.GetType().GetRuntimeProperty(att.AssociatedPropertyName);
                var associatedPropValue = associatedPropInfo.GetValue(this);
                if ((bool)associatedPropValue)
                {
                    var imgList = propValue as ObservableCollection<ImageCapture>;
                    if (imgList != null)
                    {
                        if (imgList.Count == 0)
                        {
                            if (!_errors.Any(x => x.PropertyName == propInfo.Name))
                            {
                                _errors.Add(new ValidationError { PropertyName = propInfo.Name, ErrorMessage = att.ErrorMessage });
                            }
                        }
                    }
                }
            }
            return _errors.Count == 0;
        }

        async public Task<bool> VehicleDetailsImagesValidate()
        {
            if (this is PVehicleDetails || this is CVehicleDetails || this is TVehicleDetails)
            {
                var propertiesToValidate = this.GetType().GetRuntimeProperties();
                foreach (var propInfo in propertiesToValidate)
                {
                    if (propInfo.PropertyType.FullName.Equals(typeof(ImageCapture).FullName) && ((ImageCapture)propInfo.GetValue(this)).ImagePath.Contains("ms-appx:///Assets"))
                    {
                        var messageDialog = new MessageDialog("Please take missing images");
                        messageDialog.Commands.Add(new UICommand("Ok"));
                      await  messageDialog.ShowAsync();
                        return false;

                    }
                }
            }
            return true;
        }


    }
}
