using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.DocumentDelivery.UILogic.Helpers
{
    public static class DDExtension
    {
        public static ObservableCollection<T> AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> addSource)
        {
            foreach (T item in addSource)
            {
                source.Add(item);
            }

            return source;
        }
    }
}
