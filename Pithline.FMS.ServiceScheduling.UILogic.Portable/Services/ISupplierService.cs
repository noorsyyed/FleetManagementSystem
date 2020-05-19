using Pithline.FMS.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Services
{
    public interface ISupplierService
    {
         Task<ObservableCollection<Supplier>> GetSuppliersByClassAsync(string classId, UserInfo userInfo);

         Task<bool> InsertSelectedSupplierAsync(SupplierSelection supplierSelection, UserInfo userInfo);

         Task<ObservableCollection<Supplier>> SearchSupplierByLocationAsync(string countryId, string provinceId, string cityId, string suburbId, string regionId, UserInfo userInfo);
       
    }
}
