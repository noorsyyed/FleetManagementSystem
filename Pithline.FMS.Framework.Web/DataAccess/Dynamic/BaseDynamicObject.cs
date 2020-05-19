using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.DataAccess.Dynamic
{
    public abstract class BaseDynamicObject : IDynamicObject
    {
        object IDynamicObject.CallMethod(string methodname, object[] paramList)
        {
            return this.CallMethod(methodname, paramList);
        }

        protected virtual object CallMethod(string methodname, object[] paramList)
        {
            try
            {
                var method = this.GetType().GetMethod(methodname);
                if (method != null)
                {
                    return method.Invoke(this, paramList);
                }
                else
                {
                    throw new Exception(string.Format("The method {0} not found in {1}", methodname, this.GetType().FullName));
                }
            }
            catch (AmbiguousMatchException ex)
            {
                throw new Exception(string.Format("In the class {0} you have more that one method named {1}", this.GetType().FullName, methodname),ex);
            }
        }
    }
}
