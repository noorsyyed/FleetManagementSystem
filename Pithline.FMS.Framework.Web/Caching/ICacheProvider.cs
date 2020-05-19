using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Caching
{
    public interface ICacheProvider
    {
        object this[string key] { get; set; }
    }
}
