using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pithline.FMS.BusinessLogic.Portable.TIModels
{
    public class MaintenanceRepair : BindableBase
    {
        public MaintenanceRepair()
        {
            this.IsMajorPivot = true;
            this.MajorComponentImgList = new ObservableCollection<ImageCapture>();
            this.SubComponentImgList = new ObservableCollection<ImageCapture>();
        }

        private ObservableCollection<ImageCapture> majorComponentImgList;
        public ObservableCollection<ImageCapture> MajorComponentImgList
        {
            get { return majorComponentImgList; }
            set { SetProperty(ref majorComponentImgList, value); }
        }

        private ObservableCollection<ImageCapture> subComponentImgList;
        public ObservableCollection<ImageCapture> SubComponentImgList
        {
            get { return subComponentImgList; }
            set { SetProperty(ref subComponentImgList, value); }
        }
        private bool isMajorPivot;
        public bool IsMajorPivot
        {
            get { return isMajorPivot; }
            set { SetProperty(ref isMajorPivot, value); }
        }

        private long repairid;

        public long Repairid
        {
            get { return repairid; }
            set { repairid = value; }
        }


        private long caseServiceRecId;

        public long CaseServiceRecId
        {
            get { return caseServiceRecId; }
            set { caseServiceRecId = value; }
        }


        private string majorComponent;

        public string MajorComponent
        {
            get { return majorComponent; }
            set { majorComponent = value; }
        }


        private string subComponent;

        public string SubComponent
        {
            get { return subComponent; }
            set { subComponent = value; }
        }

        private string cause;
        public string Cause
        {
            get { return cause; }
            set { cause = value; }
        }

        private string action;

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

    }



  
}
