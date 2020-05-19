using Pithline.FMS.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Services
{
    public interface IServiceDetailService
    {
         Task<bool> InsertServiceDetailsAsync(ServiceSchedulingDetail serviceSchedulingDetail, Address address, UserInfo userInfo);

         Task<ServiceSchedulingDetail> GetServiceDetailAsync(string caseNumber, long caseServiceRecId, long serviceRecId, UserInfo userInfo);

         Task<ObservableCollection<string>> GetServiceTypes(string caseNumber, string companyId);
        Task< ObservableCollection<LocationType>> GetLocationType(long serviceRecId, string companyId);
        Task< ObservableCollection<DestinationType>> GetDestinationTypeList(string callerKey, string cusId, UserInfo userInfo);
    }
}
