using Pithline.FMS.BusinessLogic.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.Common
{
    public class PropertyHistory
    {

        private static readonly PropertyHistory instance = new PropertyHistory();
        public Dictionary<string, object> StorageHistory = new Dictionary<string, object>();
        public Dictionary<string, Dictionary<string, object>> StorageHistoryMap = new Dictionary<string, Dictionary<string, object>>();
        public static PropertyHistory Instance
        {
            get
            {
                return instance;
            }
        }
        public void SetPropertyHistory(BaseModel baseModel)
        {

            try
            {
                StorageHistory.Clear();
                TypeInfo typeInfo = baseModel.GetType().GetTypeInfo();
                IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;
                propertyInfoList.AsParallel().ForAll(propInfo =>
                {
                    object value = propInfo.GetValue(baseModel);
                    StorageHistory.Add(propInfo.Name, value);
                });
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SetPropertyHistory(ObservableCollection<TTyreCond> baseModels)
        {

            try
            {
                StorageHistoryMap.Clear();
                foreach (var baseModel in baseModels)
                {
                    StorageHistory = new Dictionary<string, object>();
                    TypeInfo typeInfo = baseModel.GetType().GetTypeInfo();
                    IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;
                    foreach (var propInfo in propertyInfoList)
                    {
                        object value = propInfo.GetValue(baseModel);
                        StorageHistory.Add(propInfo.Name, value);

                    }
                    StorageHistoryMap.Add(baseModel.Position, StorageHistory);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsPropertiesOriginalValuesChanged(ObservableCollection<TTyreCond> contexts)
        {
            try
            {
                bool ischanged = false;

                foreach (var context in contexts)
                {
                    TypeInfo typeInfo = context.GetType().GetTypeInfo();
                    IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;

                    foreach (var propInfo in propertyInfoList)
                    {
                        string currentValue = Convert.ToString(propInfo.GetValue(context));
                        object originalvalue;
                        var StorageHistory = StorageHistoryMap[context.Position];
                        StorageHistory.TryGetValue(propInfo.Name, out originalvalue);
                        if (!currentValue.Equals(Convert.ToString(originalvalue)))
                        {
                            ischanged = true;
                        }
                    }

                }
                return ischanged;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool IsPropertyOriginalValueChanged(object context)
        {
            try
            {
                TypeInfo typeInfo = context.GetType().GetTypeInfo();
                IEnumerable<PropertyInfo> propertyInfoList = typeInfo.DeclaredProperties;

                foreach (var propInfo in propertyInfoList)
                {
                    string currentValue = Convert.ToString(propInfo.GetValue(context));
                    object originalvalue;
                    StorageHistory.TryGetValue(propInfo.Name, out originalvalue);
                    if (!currentValue.Equals(Convert.ToString(originalvalue)))
                    {
                        return true;

                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
