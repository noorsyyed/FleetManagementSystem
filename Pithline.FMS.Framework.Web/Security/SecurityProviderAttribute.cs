using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Security
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SecurityProviderAttribute:ExportAttribute
    {
        public SecurityProviderAttribute() : base(typeof(ISecurityProvider)) { }

        public string Name { get; set; }
    }
}
