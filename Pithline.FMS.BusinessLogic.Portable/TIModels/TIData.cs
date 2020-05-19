
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Portable.TIModels
{
    public class TIData : BindableBase
    {
        public TIData()
        {
            this.CompletionDate = DateTime.Today;
        }
        private string causeOfDamage;

        public string CauseOfDamage
        {
            get { return causeOfDamage; }

            set { SetProperty(ref causeOfDamage, value); }
        }

        private string remedy;

        public string Remedy
        {
            get { return remedy; }

            set { SetProperty(ref remedy, value); }
        }

        private string recommendation;

        public string Recommendation
        {
            get { return recommendation; }

            set { SetProperty(ref recommendation, value); }
        }

        private DateTime completionDate;

        public DateTime CompletionDate
        {
            get { return completionDate; }

            set { SetProperty(ref completionDate, value); }
        }
        private long caseServiceRecID;

        public long CaseServiceRecID
        {
            get { return caseServiceRecID; }

            set { SetProperty(ref caseServiceRecID, value); }
        }

        private ObservableCollection<object> caseCategoryAuthList;

        public ObservableCollection<object> CaseCategoryAuthList
        {
            get { return caseCategoryAuthList; }

            set { SetProperty(ref caseCategoryAuthList, value); }
        }

        private bool shouldSave;

        public bool ShouldSave
        {
            get { return shouldSave; }

            set { SetProperty(ref shouldSave, value); }
        }

    }
}
