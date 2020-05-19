using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    class DamageSnapshotRequiredAttribute : Attribute
    {
        public DamageSnapshotRequiredAttribute(string errorMessage,string  associatedPropertyName)
        {
            _errorMessage = errorMessage;
            _associatedPropertyName = associatedPropertyName;
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        private string _associatedPropertyName;

        public string AssociatedPropertyName
        {
            get { return _associatedPropertyName; }
            set { _associatedPropertyName = value; }
        }

    }
}
