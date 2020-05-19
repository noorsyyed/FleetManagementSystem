using Pithline.FMS.BusinessLogic;
using Pithline.FMS.BusinessLogic.Base;
using Pithline.FMS.BusinessLogic.Passenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using Pithline.FMS.BusinessLogic.Commercial;

namespace Pithline.FMS.VehicleInspection
{
    public static class VIExtension
    {
        public static void LoadSnapshotsFromDb(this BaseModel viBase)
        {

            try
            {
                TypeInfo t = viBase.GetType().GetTypeInfo();
                IEnumerable<FieldInfo> fieldInfoList = t.DeclaredFields;
                IEnumerable<PropertyInfo> propertyInfoList = t.DeclaredProperties;
                foreach (var fieldInfo in fieldInfoList.Where(x => x.Name.Contains("Path")))
                {
                    string pathValue =Convert.ToString(t.GetDeclaredField(fieldInfo.Name).GetValue(viBase));
                    if (!String.IsNullOrEmpty(pathValue))
                    {
                        var prop = propertyInfoList.First(x => x.Name.ToUpper().Equals(fieldInfo.Name.Replace("Path", "").ToUpper()));
                        if (prop.PropertyType.Equals(typeof(ObservableCollection<ImageCapture>)))
                        {
                            ObservableCollection<ImageCapture> imgListvalue = new ObservableCollection<ImageCapture>();
                            string[] pathlist = pathValue.ToString().Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string path in pathlist)
                            {
                                imgListvalue.Add(new ImageCapture() { ImagePath = path });
                            }
                            prop.SetValue(viBase, imgListvalue);
                        }
                        else if (prop.PropertyType.Equals(typeof(ImageCapture)))
                        {
                            prop.SetValue(viBase, new ImageCapture() { ImagePath = pathValue.ToString() });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
