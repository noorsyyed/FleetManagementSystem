using Pithline.FMS.BusinessLogic.Portable.SSModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Services
{
    public interface ILocationService
    {
        Task<ObservableCollection<Country>> GetCountryList(UserInfo userInfo);
        Task<ObservableCollection<Province>> GetProvinceList(string countryId, UserInfo userInfo);
        Task<ObservableCollection<City>> GetCityList(string countryId, string stateId, UserInfo userInfo);
        Task<ObservableCollection<Suburb>> GetSuburbList(string countryId, string stateId, UserInfo userInfo);
        Task<ObservableCollection<Region>> GetRegionList(string countryId, string stateId, UserInfo userInfo);
        Task<ObservableCollection<string>> GetZipcodeList(string countryId, string stateId, UserInfo userInfo);
    }
}
