using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.DataAccess
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DataProviderAttribute:ExportAttribute
    {
        public DataProviderAttribute() : base(typeof(IDataProvider)) { }
        public string Name { get; set; }
    }
}
