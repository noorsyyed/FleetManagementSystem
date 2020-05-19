using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Caching
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class CacheProviderAttribute : ExportAttribute
    {
        public CacheProviderAttribute() : base(typeof(ICacheProvider)) { }
        public string Name { get; set; }
    }
}
