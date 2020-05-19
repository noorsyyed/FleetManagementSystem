using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Pithline.FMS.DataProvider.AX.TI
{
    public class MaintenanceRepair
    {
        public MaintenanceRepair()
        {

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
