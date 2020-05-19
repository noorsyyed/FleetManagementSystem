using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Helpers
{
    public static class SSExtension
    {
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> destination, IEnumerable<T> source)
        {
            if (destination != null && source != null)
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
            return destination;
        }
    }
}
