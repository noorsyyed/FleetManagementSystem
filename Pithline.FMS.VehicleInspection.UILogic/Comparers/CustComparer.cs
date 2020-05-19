using Pithline.FMS.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.VehicleInspection.UILogic.Comparers
{
public    class CustComparer : IEqualityComparer<Customer>
    {
        public bool Equals(Customer x, Customer y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Customer obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
