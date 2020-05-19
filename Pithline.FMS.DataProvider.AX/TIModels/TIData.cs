
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DataProvider.AX.TI
{
    public class TIData
    {
        private string causeOfDamage;

        public string CauseOfDamage
        {
            get { return causeOfDamage; }
            set { causeOfDamage = value; }
        }        

        private string remedy;

        public string Remedy
        {
            get { return remedy; }
            set { remedy = value; }
        }

        private string recommendation;

        public string Recommendation
        {
            get { return recommendation; }
            set { recommendation = value; }
        }

        private DateTime completionDate;

        public DateTime CompletionDate
        {
            get { return completionDate; }
            set { completionDate = value; }
        }
        private long caseServiceRecID;

        public long CaseServiceRecID
        {
            get { return caseServiceRecID; }
            set { caseServiceRecID = value; }
        }

        private ObservableCollection<object> caseCategoryAuthList;

        public ObservableCollection<object> CaseCategoryAuthList
        {
            get { return caseCategoryAuthList; }
            set { caseCategoryAuthList = value; }
        }

        private bool shouldSave;

        public bool ShouldSave
        {
            get { return shouldSave; }
            set { shouldSave = value; }
        }
    }
}
