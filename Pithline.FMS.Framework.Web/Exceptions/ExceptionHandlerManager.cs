using Pithline.FMS.Framework.Web.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Exceptions
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ExceptionHandlerManager
    {
        [ImportMany]
        public Lazy<IExceptionHandler,IExceptionHandlerMetadata>[] Handlers { get; set; }

        public void Handle(Exception ex)
        {
            var type = MefHelper.Helper.App.ExceptionHandlerType;
            var handler = (from h in this.Handlers where h.Metadata.Type.Equals(type) select h).FirstOrDefault();
            if (handler != null)
            {
                handler.Value.Handle(ex);
            }
            //else{
            //TODO: Handle the else part of it
            
        }
    }
}
