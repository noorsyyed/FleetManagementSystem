using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Exceptions
{
    public interface IExceptionHandlerMetadata
    {
        string Type { get; }
    }
}
