using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.DataAccess.Dynamic
{
    /// <summary>
    /// This attribute is used in objects which are going to be called dynamically via reflection from the client
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DynamicObjectAttribute : ExportAttribute
    {
        public DynamicObjectAttribute() : base(typeof(IDynamicObject)) { }
        public string Name { get; set; }
    }
}
